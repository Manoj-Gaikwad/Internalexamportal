using System;
using System.Collections.Generic;
using System.Text;

namespace Internalexamportal.Core.Commons
{
    public class ExamLinkTemplate
    {
        public const string ExamTemplate = @"
        <div id='mainDiv'>
	        <div style = 'padding: 10px; background-color: #C8E5F6; -webkit-border-radius: 10px;-moz-border-radius: 10px; border-radius: 10px;' >
		        <div style='-webkit-border-radius: 10px; -moz-border-radius: 10px; border-radius: 10px;'>

			        <div style = 'padding: 1%; background-color: #F1FAFF; font-family: Arial, Helvetica, sans-serif;font-size: 14px; font-style: normal; font-variant: normal; font-weight: normal;' >
				        <div id='User' style='padding-top: 1%; height: 30px; font-size:16px;'>
					        <p></p>
					        <h2>@Activation</h2>
				        </div>
				        <div id = 'bodyDiv' style='font-family: Arial, Helvetica, sans-serif; font-size: 14px; background-color: #F1FAFF; font-style: normal; font-variant: normal;
                 font-weight: normal;padding-top: 1%;line-height:2.4;'>
					        <p> Please click the link to start exam.</p>
					        <a href = '@url' >Click here</a>
				        </div>
			        </div>
		        </div>
	        </div>
        </div>";

        public const string PasswordEmailTemplate = @"
			<div id='mainDiv'>
				<div style='padding: 10px; background-color: #C8E5F6; -webkit-border-radius: 10px;-moz-border-radius: 10px; border-radius: 10px;'>
					<div style='-webkit-border-radius: 10px; -moz-border-radius: 10px; border-radius: 10px;'>
						<div style='padding: 1%; background-color: #F1FAFF; font-family: Arial, Helvetica, sans-serif;font-size: 14px; font-style: normal; font-variant: normal; font-weight: normal;'>
							<div id='User' style='padding-top: 1%; height: 100px; font-size:16px;'>
								<p>Your Exam Portal Login Password</p>
								<h3>@Password</h3>
								<p> Please click the link to login.</p>
								<a href='@url'>Click here</a>
							</div>
							<div id='bodyDiv' style='font-family: Arial, Helvetica, sans-serif; font-size: 14px; background-color: #F1FAFF; font-style: normal; font-variant: normal;
								 font-weight: normal;padding-top: 1%;line-height:2.4;'>
								<br>
											</div>
							</div>
						</div>
					</div>
				</div>
			</div>";
    }
}
