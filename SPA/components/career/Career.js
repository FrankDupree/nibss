import React, { Component } from "react";
import Board from "./Board"
import Info from "./Info"
import Apply from "./Apply"
import axios from "axios"

export default class Career extends Component {
  constructor(props) {
    super(props);
    this.state = {
        jobs:[],
        showBoard:true,
        showInfo:false,
        showForm:false,
        showResult:false,
        isSuccess:false,
        selectedJob:{Description:""}
    };

    this.getJobs();
  }

  getJobs =()=>{
    var self = this;

  axios.get('/careers/init')
      .then(function (response) {
        // handle success
        self.setState({jobs:response.data})
      })
      .catch(function (error) {
        // handle error
      })
      .finally(function () {
        // always executed
      });
  }

  apply=(id)=>{
    var job = this.state.jobs.find(x=> x.Id == id);
    this.setState({selectedJob:job, showBoard:false, showInfo:true});
  }

  applyForm=(job)=>{
    this.setState({showInfo:false, showForm:true});
  }

  submitForm=(job)=>{
    this.setState({showInfo:false, showForm:true});
  }

  showResult=(isSuccess)=>{
    this.setState({isSuccess:isSuccess},function(){
        this.setState({showForm:!isSuccess, showResult:true});
    });
  }

  reset=(e)=>{
    e.preventDefault();
    this.setState({showResult:false, showBoard:true});
  }

  render() {
    return (
      <div>
         <div style={{"display": this.state.showBoard ? "block": "none"}}>
            <Board jobs={this.state.jobs} apply={this.apply}/>
         </div>
        <div style={{"display": this.state.showInfo ? "block": "none"}}>
            <Info job={this.state.selectedJob} apply={this.applyForm}/>
        </div>
        <div style={{"display": this.state.showForm ? "block": "none"}}>
            {this.state.showForm ? <Apply showResult={this.showResult} job={this.state.selectedJob} apply={this.submitForm}/>:""}
        </div>
         <div style={{"display": this.state.showResult ? "block": "none"}}>
           <div className="uk-container uk-container-small uk-margin-medium-top">
              {this.state.isSuccess ?
                <div className="uk-placeholder uk-text-center">
                      <strong>Application submission successfull....Goodluck!</strong>.
                      <p>
                          <a onClick={this.reset} className="uk-button uk-button-default uk-button-small uk-text-capitalize xftyq" href="#">
                              GO BACK
                          </a>
                      </p>
                </div>:
                <div className="uk-placeholder uk-text-center">
                    <strong style={{"color":"red"}}>Error sending message...please try again!</strong>.
                </div>
              }
             </div>
         </div>
      </div>
    );
  }
}
