import React, { Component } from "react";
import parse from 'html-react-parser'

export default class Info extends Component {
  constructor(props) {
    super(props);
  }
  

  render() {
    return (
      <div>
       <div className="uk-container uk-container-small uk-margin-medium-top">
           <p className="uk-heading-line uk-text-small"><span>WE CARE FOR OUR TEAM</span></p>
           <h4>Careers > {this.props.job.Role}</h4>
           <div className="uk-child-width-expand@s uk-text-left" data-uk-grid>
            <div>
              {parse(this.props.job.Description)}
            </div>
            <div className="uk-width-1-5@m">
                <button onClick={()=>this.props.apply(this.props.job)} className="uk-button uk-button-large xftyq">Apply</button>
            </div>
        </div>
        </div>
      </div>
    );
  }
}
