# GSF.Matters
[![Build Status](https://teamcity.globalx.com.au/app/rest/builds/buildType:(id:Gsf_Matters_Build)/statusIcon)](https://teamcity.globalx.com.au/viewType.html?buildTypeId=Gsf_Matters_Build&guest=1)

An API to manage leasing matters in VNEXT

## Local Development

### Get Local Access Token for GC Secrets
```
gcloud auth print-access-token --impersonate-service-account matters@glx-development-au.iam.gserviceaccount.com
```
Add this token to your IDE Environment Variable DEVELOPER_ACCESS_TOKEN

### Database
#### PostgreSQL Database setup
```shell
docker pull postgres:12-alpine
docker run --name gsf_matters -e POSTGRES_PASSWORD=ThisSureIsAPassword -d -p 5432:5432 postgres:12-alpine
```

#### Start Database
```
docker start gsf_matters
```

### Create a new migration
Add a new SQL file to the Migrations folder of the Database project. Make sure to mark it as an embedded resource in csproj file, otherwise it will not get run when the migrations are run.

Any new tables will need their models added manually to the Models folder of the database project, and modifications to the existing classes will need to be done manually as well.

### Apply migrations
Run the Endpoint project with the args set to `execute-migrations`. There are two ways to set arguments:
1. Run -> Edit Configurations -> Program Arguments
2. If Program Arguments are read only. Go to `launchSettings.json` in `Properties` folder in Endpoint project, add `"commandLineArgs": "execute-migrations"` under `GSF.Matters.Endpoint` attribute

In higher environments this is done via an init container in kubernetes. For local development, you may need to adjust the password it uses to connect to your local database.

## Development
### Regenerating controllers
To regenerate the controllers based on an OpenApi specifications, do the following:
1. Run the swagger locally using kong and turn off auth (make sure you can hit `https://localdev.globalx.com.au/documentation/apis/gsf-matters/swagger.yaml` in Postman without gxid cookie)
2. Run `gcloud auth login` and `gcloud auth configure-docker`
3. Change the placeholder string in the powershell script `regenerate-controller-and-client-from-swagger.sh` from `<your_ip>` to your local IP address
4. Run the script `./regenerate-controller-and-client-from-swagger.sh`

OR via Nswag command line rather than sh script (which utilises NSwag docker image):
To regenerate the controllers based on an updated swagger document, do the following:
1. Install NSwag.ConsoleCore globally - https://www.nuget.org/packages/NSwag.ConsoleCore/
        ```
        dotnet tool install --global NSwag.ConsoleCore --version 13.9.4
        ```
    
3. Make sure matter-nswag.json is pointing to correct source location - either url or file path to swagger.yml
   a. if URL Run the swagger locally using kong and turn off auth (make sure you can hit `https://localdev.globalx.com.au/documentation/apis/swagger-ui/?url=/documentation/apis/gsf-matters/swagger.yaml` without authing)
2. run command to generate controller
        ```
        nswag run matters-nswag.json
        ```
   
### Run Locally
1. In Konga, change upstream URL of `gsf-matters-service.svc` to `localdev.globalx.com.au:8358` or `<your-ip>:8358`
2. Update the request transformer plugin property for the 'gsf-matters-route' route as below:\
Change the URI in the *replace* section to `/v1$(uri_captures[1])`
3. Update the request transformer plugin property for the 'gsf-matters-workflows-route' route as below:\
Change the URI in the *replace* section to `/workflows$(uri_captures[1])`


## Version bumps
This project has been migrated to semantic release, so no dockerfile version bumps are necessary.

## Deployment
Please update the image version in the gsf-services-deployment to deploy to staging and production via flux

## Related Projects
- GSF.Workflow
- GSF.Contacts
