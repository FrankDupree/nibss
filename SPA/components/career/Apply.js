import React, { Component } from "react";
import parse from 'html-react-parser'
import Board from "./Board"
import axios from "axios"
import {is_email, isEmpty, hasNumber} from '../util'

export default class Apply extends Component {
  constructor(props) {
    super(props);
    this.state = {
        firstName:"",
        surname:"",
        phone:"",
        email:"",
        deg:[],
        degree:"Select a degree",
        selectedFile: null,
        errors: { firstName: '', surname: '', phone: '', email: '', cv: ''}
    };

    this.getDegrees();

  }

  componentDidMount(){
    $(document).on('click', '#updt', function(event) { 
        event.preventDefault(); 
        $("#cvv").click(); 
    });
  }

   handleChange=(event)=> {
    this.setState({[event.target.name]: event.target.value});
  }

  onChangeHandler=event=>{
    var formIsValid = true;

    let errors = {firstName: '', surname: '', phone: '', email: '', cv: ''};
      var isValidFile = this.checkMimeType(event, errors);
      if(!isValidFile)formIsValid=false;

      this.setState({ errors });

   this.setState({
      selectedFile: event.target.files[0]
    })

}

checkMimeType=(event, errors)=>{
  //getting file object
  let files = event.target.files 
  //define message container
  let err = ''
  // list allow mime type
 const types = ['application/msword', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', 'application/pdf']
  // loop access array
  for(var x = 0; x<files.length; x++) {
   // compare file type find doesn't matach
       if (types.every(type => files[x].type !== type)) {
       // create error message and assign to container   
       err += files[x].name+' is not a supported format\n';
     }
   };

 if (err !== '') { // if message not same old that mean has error 
      event.target.value = null // discard selected file
      errors.cv = err;
       return false; 
  }
 this.setState({selectedFile:files});
 return true;

}

  getDegrees =()=>{
    var self = this;
    axios.get("/careers/GetDegrees", {
          onUploadProgress: ProgressEvent => {
          },
        }).then(res => { // then print response status
          self.setState({deg:res.data});
        }).catch(err => { // then print response status
        })
    }

  apply=(e)=>{
    var self = this;

    var formIsValid = true;
    //run validation
    e.preventDefault();

      const { firstName, surname, phone, email, degree} = this.state;
      let errors = {firstName: '', surname: '', phone: '', email: ''};

       if (!firstName || isEmpty(firstName)) {
        errors.firstName = 'First name is required';
        formIsValid = false;
      }
      if (hasNumber(firstName)) {errors.firstName = 'Name cant have numbers';formIsValid = false;}

       if (!phone) {
        errors.phone = 'Phone number is required';
        formIsValid = false;

      }else{
        var pattern = new RegExp(/^[0-9\b]+$/);
        if (!pattern.test(phone)) {
          formIsValid = false;
          errors.phone = "Please enter only number.";
        }else if(phone.length < 10 || phone.length > 11){
          formIsValid = false;
          errors.phone = "Please enter valid phone number.";
        }
      }

       if (!surname || isEmpty(surname)) {
        errors.surname = 'Surname is required';
        formIsValid = false;

      }
      if (hasNumber(surname)) {errors.surname = 'Surname cant have numbers';formIsValid = false;}


      if (!email) {
        errors.email = 'Email is required';
        formIsValid = false;
      }else{
         if(!is_email(email)){
          errors.email = 'Please enter a valid email address';
          formIsValid = false;
        }

      }

      if (degree == "Select a degree" || degree == "0") {
        errors.degree = 'Please select a valid degree';
        formIsValid = false;

      }

       if (self.state.selectedFile == null) {
        errors.cv = 'C.V must be attached!';
        formIsValid = false;
      }


      this.setState({ errors });

      if(formIsValid){
        const data = new FormData() 
        data.append('cv', this.state.selectedFile);
        data.append('FirsName', this.state.firstName);
        data.append('phone', this.state.phone);
        data.append('email', this.state.email);
        data.append('surname', this.state.surname);
        data.append('role', this.props.job.Role);
        data.append('degree', this.state.degree);
        data.append('__RequestVerificationToken', $('input[name$="__RequestVerificationToken"]')[0].value)
        axios.post("/careers/submitcv", data, {
          onUploadProgress: ProgressEvent => {
          },
        }).then(res => { // then print response status
            self.props.showResult(res.data.success);
        }).catch(err => { // then print response status
            alert("Something went wrong, please try again later");
        })
    }

  }

  render() {
     const { errors } = this.state;

    return (
      <div>
       <div className="uk-container uk-container-small uk-margin-medium-top ct-frx">
           <p className="uk-heading-line uk-text-small uk-width-1-5"><span>CAREERS FORMS</span></p>
           <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod
tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,</p>
           <div className="uk-child-width-expand@s uk-text-left" data-uk-grid>
            <div>
              <form className="uk-grid-small" data-uk-grid>
                 
                  <div className="uk-width-1-2@s">
                      <label className="uk-form-label" htmlFor="form-stacked-select">First Name</label>
                      <input name="firstName" value={this.state.firstName} onChange={this.handleChange} className="uk-input" type="text" />
                      {errors.firstName != '' && <span style={{color: "red"}}>{this.state.errors.firstName}</span>}
                  </div>
                  <div className="uk-width-1-2@s">
                      <label className="uk-form-label" htmlFor="form-stacked-select">Surname</label>
                      <input name="surname" value={this.state.surname} onChange={this.handleChange} className="uk-input" type="text" />
                      {errors.surname != '' && <span style={{color: "red"}}>{this.state.errors.surname}</span>}
                  
                  </div>
                   <div className="uk-width-1-2@s">
                      <label className="uk-form-label" htmlFor="form-stacked-select">Email address</label>
                      <input name="email" value={this.state.email} onChange={this.handleChange} className="uk-input" type="text" />
                      {errors.email != '' && <span style={{color: "red"}}>{this.state.errors.email}</span>}
                  
                  </div>
                  <div className="uk-width-1-2@s">
                      <label className="uk-form-label" htmlFor="form-stacked-select">Phone number</label>
                      <input name="phone" value={this.state.phone} onChange={this.handleChange} className="uk-input" type="text" />
                      {errors.phone != '' && <span style={{color: "red"}}>{this.state.errors.phone}</span>}
                  
                  </div>
                   <div className="uk-width-1-1">
                      <label className="uk-form-label" htmlFor="form-stacked-select">Position you are applying for</label>
                      <input name="role" value={this.props.job.Role} className="uk-input" type="text" />
                  </div>
                   <div className="uk-width-1-1">
                      <label className="uk-form-label" htmlFor="form-stacked-select">Highest Qualification</label>
                      <div className="uk-form-controls">
                          <select name="degree" className="uk-select" id="form-stacked-select" onChange={this.handleChange}>
                          {
                            this.state.deg.map((i,x)=>{
                              return (<option value={i.value}>{i.key}</option>)
                            })
                          }
                          </select>
                          {errors.degree != '' && <span style={{color: "red"}}>{this.state.errors.degree}</span>}
                  </div>
                  <br/>
                  <div className="uk-width-1-1">
                      <label className="uk-form-label" htmlFor="form-stacked-select">Upload CV (.docx)</label>
                      <br/>
                      {errors.cv != '' && <span style={{color: "red"}}>{this.state.errors.cv}</span>}
                      <div className="up">
                        <div className="t">
                          <div uk-form-custom="target: true" style={{"width":"inherit"}}>
                              <input id="cvv" name="cv" type="file" onChange={this.onChangeHandler}/>
                              <input  className="uk-input" type="text" placeholder="Select file" disabled/>                                  
                          </div>
                        </div>
                        <input id="updt" className="uk-input uk-button xftyq" type="button" value="Upload"/>
                      </div>
                  </div>
                  <div className="uk-width-1-1 uk-text-center uk-margin-medium-top">
                      <button onClick={this.apply} className="uk-button uk-button-medium xftyq">Apply</button>
                  </div>
                </div>
              </form>
            </div>
            <div className="uk-width-1-5@m">
              <a href="/Careers"><div className="ctok">
                <div className="uk-clearfix">
                    <div className="uk-float-left">
                        Careers
                    </div>
                    <div className="uk-float-right">
                        <span uk-icon="chevron-right" style={{"color":"rgb(158, 125, 19)"}}></span>
                    </div>
                    </div>
                </div>
              </a>
              <a href="/about"><div className="ctok">
                <div className="uk-clearfix">
                    <div className="uk-float-left">
                        About Us
                    </div>
                    </div>
                </div>
              </a>
            </div>
        </div>
        </div>
      </div>
    );
  }
}
