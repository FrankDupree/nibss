using System.Threading.Tasks;
using Account.Models;
using Account.ViewModels;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Users.Models;

namespace nibss_orchad_azure.Drivers
{
    public class UserProfileDisplayDriver : SectionDisplayDriver<User, UserProfile>
    {
        public override IDisplayResult Edit(UserProfile section, BuildEditorContext context)
        {
            return Initialize<EditUserProfileViewModel>("UserProfile_Edit", model =>
            {
                model.Sector = section.Sector;
                model.Organisation = section.Organisation;
                model.Name = section.Name;
            }).Location("Content:2");
        }

        public override async Task<IDisplayResult> UpdateAsync(UserProfile section, BuildEditorContext context)
        {
            var model = new EditUserProfileViewModel();

            if (await context.Updater.TryUpdateModelAsync(model, Prefix))
            {
                model.Sector = section.Sector;
                model.Organisation = section.Organisation;
                model.Name = section.Name;
            }

            return Edit(section, context);
        }
    }
}
