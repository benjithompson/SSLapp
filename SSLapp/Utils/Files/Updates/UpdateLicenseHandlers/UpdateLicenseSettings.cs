using System;
using System.Collections.Generic;
using System.Text;
using SSLapp.Models;
using SSLapp.Utils.Files.Update;
using SSLapp.Utils.Files.Updates;

namespace SSLapp.Utils.Files.Updates.UpdateLicenseHandlers
{

    /*
     * 1. Export your SSL-Certificate from IIS-Manager
     * 2. export your certificate to any folder. Tricentis recommends that you use the default License Server directory: C:\Program Files\TRICENTIS\Tricentis License Server 
     * 3. cmd C:\Program Files\TRICENTIS\Tricentis License Server\License Server\2017.8.0\jre1.8.0_191\bin          (%JAVA_HOME%\bin to run keytool)
     *    - Convert your PFX file by typing the following string:  
     *    
     *    cert.pfx -srcstoretype pkcs12 -destkeystore "C:\Program Files\TRICENTIS\Tricentis License Server\<NameOfChoice>.jks -deststoretype JKS
     *    
     *    Enter password for pfx file and set password for the keystore .jks file (same as in step 2)
     *    
     * 4. cmd C:\Program Files\TRICENTIS\Tricentis License Server\License Server\2017.8.0\License Server Deployment\flexnetls-x64_windows-2017.08.0\server
     *    -flexnetls.bat -stop
     *    
     * 5. cmd C:\Program Files\TRICENTIS\Tricentis License Server\License Server\2017.8.0\License Server Deployment\flexnetls-x64_windows-2017.08.0\server
     *    -edit file flexnetls.settings: change PORT to 0
     *    
     * 6. -edit local-configuration.yaml
     *        -https-in-enabled 'true'
     *        -default port is 1443, change if needed
     *        -Enter the path to your .jks file. Use double-backslashes instead of single backslashes.
     *        -Enter the keystore password you defined in step 3
     *        -
     * 7. Update Flexera with another command prompt: flexnetls.bat -update
     * 8. Restart Flexera with flexnetls.bat -start.
     */

    class UpdateLicenseSettings : IUpdateFilesBehavior
    {
        public UpdateLicenseSettings(string appPath)
        {
            AppPath = appPath;
            UpdatedFilesCount = 0;
            Updated = false;
        }

        public string AppPath { get; set ; }

        public bool Updated {get; set; }

        public int UpdatedFilesCount { get; set; }

        public void Update(ToscaConfigFilesModel config)
        {
            throw new NotImplementedException();
        }
    }
}
