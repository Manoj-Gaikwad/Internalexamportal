using System;
using System.Collections.Generic;
using System.Text;

namespace Internalexamportal.Core.Commons
{
    public class EmailBodyTemplates
    {
        public const string forgotPasswordTemplate = @"
			<div id='mainDiv'>
				<div style = 'padding: 10px; background-color: #C8E5F6; -webkit-border-radius: 10px;-moz-border-radius: 10px; border-radius: 10px;' >
					<div style='-webkit-border-radius: 10px; -moz-border-radius: 10px; border-radius: 10px;'>

						<div style = 'padding: 1%; background-color: #F1FAFF; font-family: Arial, Helvetica, sans-serif;font-size: 14px; font-style: normal; font-variant: normal; font-weight: normal;' >

							<div id = 'bodyDiv' style='font-family: Arial, Helvetica, sans-serif; font-size: 14px; background-color: #F1FAFF; font-style: normal; font-variant: normal; font-weight: normal;padding-top: 1%;line-height:2.4;'>
								<p> Please click the link to Reset your password.</p>
								<a href = '@here' > Click here</a>
							</div>
						</div>
					</div>
				</div>
			</div>";
    }
}
