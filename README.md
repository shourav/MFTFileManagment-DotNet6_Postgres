# MFTFileManagment-DotNet6_Postgres
1. Install the rebex sftp server  (sample install file is located in the RequiredFilesForProject folder)
2. You may need to get to different license key from their website for using rebex sftp
3. This solution is based on 2 projects:
    a. MFTFileManagement (web api project)
    b. APISchedulerService (windows worker project)
4. Please change required settings from both projects' appsettings.json files
5. Entity framework has been used along with Postgres database (code first approach) 
6. DownloadFilesFromServer API can be called independently to download files from SFTP server
7. In order to download files from the SFTP every 1 minute please run the APISchedulerService program (it is configured to call the API every 1 minute to download files)
  
