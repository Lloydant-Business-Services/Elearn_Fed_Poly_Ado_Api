using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class ZoomParam
    {
        //https://zoom.us/oauth/authorize?response_type=code&client_id=7lstjK9NTyett_oeXtFiEQ&redirect_uri=https://yourapp.com
        public string ClientId { get; set; } = "Onu9AxEGRKCOxSXagsJizA";//Onu9AxEGRKCOxSXagsJizA
        //public string ClientId { get; set; } = "wUVZp6VWSNerRXBLFtyLfw";//Onu9AxEGRKCOxSXagsJizA
        //public string ClientSecret { get; set; } = "gZ4V9Z86d9LZri2NL6g33J44LFO9mL8T";//7MoHxY31v9tzG5zhncirgr266EDKUNAN
        public string ClientSecret { get; set; } = "7MoHxY31v9tzG5zhncirgr266EDKUNAN";//7MoHxY31v9tzG5zhncirgr266EDKUNAN

        public string AuthorizationUrl { get; set; } = "https://zoom.us/oauth/authorize?response_type=code&client_id=Onu9AxEGRKCOxSXagsJizA&redirect_uri=http://blurapp.pixufy.com/login";
        //public string AuthorizationUrl { get; set; } = "https://zoom.us/oauth/authorize?response_type=code&client_id=wUVZp6VWSNerRXBLFtyLfw&redirect_uri=https://applications.federalpolyilaro.edu.ng/Security/Account/Login";
        
        //public string AccessTokenUrl { get; set; } = "https://zoom.us/oauth/token?grant_type=authorization_code&code={0}&redirect_uri=https://applications.federalpolyilaro.edu.ng/Security/Account/Login";
        public string AccessTokenUrl { get; set; } = "https://zoom.us/oauth/token?grant_type=authorization_code&code={0}&redirect_uri=http://blurapp.pixufy.com/login";

        public string TokenRefreshUrl { get; set; } = "https://zoom.us/oauth/token?grant_type=refresh_token&refresh_token={0}";
    }
}
