using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.DisplayManagement;
using OrchardCore.Email;
using OrchardCore.Entities;
using OrchardCore.Modules;
using OrchardCore.Settings;
using OrchardCore.Users;
using OrchardCore.Users.Events;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using OrchardCore.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MailMessage = OrchardCore.Email.MailMessage;

namespace Account.Extensions
{
    internal static class ControllerExtensions
    {

        /// <summary>
        /// Returns the created user, otherwise returns null
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="model"></param>
        /// <param name="confirmationEmailSubject"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal static async Task<IUser> RegisterUser(this Controller controller, RegisterViewModel model, string confirmationEmailSubject, ILogger logger)
        {
            var registrationEvents = controller.ControllerContext.HttpContext.RequestServices.GetRequiredService<IEnumerable<IRegistrationFormEvents>>();
            var userService = controller.ControllerContext.HttpContext.RequestServices.GetRequiredService<IUserService>();
            var settings = (await controller.ControllerContext.HttpContext.RequestServices.GetRequiredService<ISiteService>().GetSiteSettingsAsync()).As<RegistrationSettings>();
            var signInManager = controller.ControllerContext.HttpContext.RequestServices.GetRequiredService<SignInManager<IUser>>();

            if (settings.UsersCanRegister != UserRegistrationType.NoRegistration)
            {
                await registrationEvents.InvokeAsync((e, modelState) => e.RegistrationValidationAsync((key, message) => modelState.AddModelError(key, message)), controller.ModelState, logger);

                if (controller.ModelState.IsValid)
                {
                    var user = await userService.CreateUserAsync(new User { UserName = model.UserName, Email = model.Email, EmailConfirmed = !settings.UsersMustValidateEmail }, model.Password, (key, message) => controller.ModelState.AddModelError(key, message)) as User;

                    if (user != null && controller.ModelState.IsValid)
                    {
                        if (settings.UsersMustValidateEmail)
                        {
                            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                            // Send an email with this link
                            await controller.SendEmailConfirmationTokenAsync(user, confirmationEmailSubject);
                        }
                        else
                        {
                            await signInManager.SignInAsync(user, isPersistent: false);
                        }
                        logger.LogInformation(3, "User created a new account with password.");
                        registrationEvents.Invoke((e, user) => e.RegisteredAsync(user), user, logger);

                        return user;
                    }
                }
            }
            return null;
        }

        internal static async Task<bool> SendEmailAsync(this Controller controller, string email, string subject, IShape model)
        {
            var smtpService = controller.HttpContext.RequestServices.GetRequiredService<ISmtpService>();
            var displayHelper = controller.HttpContext.RequestServices.GetRequiredService<IDisplayHelper>();
            var htmlEncoder = controller.HttpContext.RequestServices.GetRequiredService<HtmlEncoder>();
            string body;

            await using (var sw = new StringWriter())
            {
                var htmlContent = await displayHelper.ShapeExecuteAsync(model);
                htmlContent.WriteTo(sw, htmlEncoder);
                body = sw.ToString();
            }

            var message = new MailMessage()
            {
                To = email,
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };




            var result = await smtpService.SendAsync(message);

            if (!result.Succeeded)
            {
                //Clever hack, if the default smtp of Orchard doesnt work, lets use the custom mailer class
                var mailService = controller.ControllerContext.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = mailService.GetValue<string>("smtp_host");
                smtpClient.Port = Int32.Parse(mailService.GetValue<string>("smtp_port"));
                System.Net.NetworkCredential smtpUserInfo = new System.Net.NetworkCredential(mailService.GetValue<string>("smtp_user"), mailService.GetValue<string>("smtp_password"));
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = smtpUserInfo;
                smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                MailAddress fromAddress = new MailAddress(mailService.GetValue<string>("supportMail"), "NIBSS");
                if (mailService.GetValue<string>("smtp_ssl").ToUpper() == "TRUE")
                {
                    smtpClient.EnableSsl = true;
                }
                else
                {
                    smtpClient.EnableSsl = false;
                }
                var Mailmessage = new System.Net.Mail.MailMessage()
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                Mailmessage.To.Add(email);
                Mailmessage.From = fromAddress;
                smtpClient.Send(Mailmessage);
                Mailmessage.Dispose();
            }

            return result.Succeeded;
        }


        internal static async Task<string> SendEmailConfirmationTokenAsync(this Controller controller, User user, string subject)
        {
            Uri host = new Uri(controller.HttpContext.Request.Headers["Origin"].ToString());
            var userManager = controller.ControllerContext.HttpContext.RequestServices.GetRequiredService<UserManager<IUser>>();
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = controller.Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, protocol: controller.HttpContext.Request.Scheme, host.Authority);
            await SendEmailAsync(controller, user.Email, subject, new ConfirmEmailViewModel() { User = user, ConfirmEmailUrl = callbackUrl });

            return callbackUrl;
        }
    }
}
