import React, { Component } from "react";
import Cv from "./Cv"

export default class Board extends Component {
  constructor(props) {
    super(props);
    this.state = {
    	showJob:false,
    	showPlain:false,
    	showCv:false
    };
  }

  componentDidMount() {
    if(this.props.jobs.length > 0){
    	this.setState({showJob:true, showPlain:false});
    }else{
    	this.setState({showJob:false, showPlain:true});
    }
  }

  showCvSubmission=(e)=>{
  	e.preventDefault();
  	this.setState({showCv:true, showPlain:false})
  }

  

  render() {
    return (
    	<div>
    		<div className="uk-container uk-container-small uk-margin-medium-top">
	    		 <p className="uk-heading-line uk-text-small"><span>WE CARE FOR OUR TEAM</span></p>
	             <h4>Careers</h4>
	             <p>NIBSS is driven by a collection of talented individuals who believe in excellence. We specialize in hiring people who have the drive to succeed and the will to implement the discipline required to succeed. We focus on nurturing our team and providing our team an environment that is conductive to creative thought. We focus on eliminating stress and pressure so our team can think clearly and creatively.</p>

				<p>We genuinely care for our team, we know that they in turn will care for our clients, partners and business.</p>

				<p>Personal impact, mentoring, and teamwork are just a few of the benefits of building a career at NIBSS</p>
    		</div>
    		<div style={{"height":"200px"}} className="uk-height-small uk-flex uk-flex-center uk-flex-middle uk-background-cover uk-light uk-margin-medium-top" data-src="/media/office.png" data-uk-img="/media/office.png">
			</div>
			<div className="uk-container uk-container-small uk-margin-medium-top">
				<h4>Why Join NIBSS</h4>
				<div className="uk-child-width-expand@s uk-text-left" data-uk-grid>
				    <div>
				  		<p className="care-pot">Flexible work schedule</p>
                        <p>We’ll help you work where you need to, when you need to. Because life happens.</p>
				    </div>
				    <div>
				        <p className="care-pot">Room to grow</p>
						<p>Learning is built into every role at NIBSS. You’ll get mentorship and take ownership</p>
				    </div>
				    <div>
				        <p className="care-pot">Flat Structure</p>
						<p>Less decision making hoops: Employee involvement is promoted by decentralizing decision making processes</p>
				    </div>
				    <div>
				        <p className="care-pot">We’ve got you covered</p>
						<p>Comprehensive healthcare, fantastic pensions scheme, education stipends, professional membership, rewards & recognition and so much more.
						</p>
				    </div>
				</div>
				<div className="uk-child-width-expand@s uk-text-left" data-uk-grid>
				    <div>
				  		<p className="care-pot">Everyone has a voice</p>
						<p>Innovation doesn’t know hierarchy. We make sure everyone is heard, considered and respected.</p>
				    </div>
				    <div>
				        <p className="care-pot">Make your move</p>
						<p>An opportunity at NIBSS will give you the ability to create a career path that matches your skills, talent, interests, and goals.</p>
				    </div>
				    <div>
				        <p className="care-pot">First Name Basis</p>
						<p>We provide a cordial, familiar and friendly work environment without rigid boundaries.</p>
				    </div>
				    <div>
				        <p className="care-pot">Our people & culture</p>
						<p>Our high-performance culture built on collaboration and innovation connects us as a team, helps us execute better our strategies and creates more engaging work relationships.</p>
				    </div>
				</div>
			
			<div style={{"display": this.state.showJob ? "block": "none"}} className="uk-overflow-auto uk-margin-large-top">
				<table className="uk-table caree-tp uk-table-striped">
				    <caption>Open Positions</caption>
				    <thead>
				        <tr>
				            <th>Role</th>
				            <th>Location</th>
				            <th>Min. Qualifications</th>
				            <th>Experience</th>
				            <th>Deadline</th>
				        </tr>
				    </thead>
				    <tbody>
				    	{
				    		this.props.jobs.map((x, i)=>{
				    			return(
				    				<tr key={x.Id}>
							            <td>{x.Role}</td>
							            <td>{x.Location}</td>
							            <td>{x.MinQ}</td>
							            <td>{x.Experience}</td>
							            <td>{moment(x.Deadline).format("MMM Do")}</td>
							            <td>
							            <a onClick={()=> this.props.apply(x.Id)} className="uk-button uk-button-default uk-button-small uk-text-capitalize button-base2" href="#">
							            APPLY
							            </a>
							            </td>
				        			</tr>
				    			)
				    		})
				    	}
				        
    				</tbody>
					</table>
				</div>
			</div>

		<div style={{"display": this.state.showPlain ? "block": "none"}}>
           <div className="uk-container uk-container-small uk-margin-medium-top">
                <div className="uk-placeholder uk-text-center">
                      <strong>No current openings at the moment...come back again!</strong>.
                      <p>
                          <a onClick={this.showCvSubmission} className="uk-button button-base2 uk-button-default uk-button-small uk-text-capitalize" href="#">
                              SUBMIT CV
                          </a>
                      </p>
                </div>
             </div>
         </div>
         {this.state.showCv ? <Cv />:""}
    	</div>

    );
  }
}
