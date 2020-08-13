remove-item ..\CoverageResults\coverage.json
remove-item .\CoverageResults\coverage.opencover.xml 

dotnet test /p:CollectCoverage=true /p:CoverletOutput=../CoverageResults/ /p:Exclude=[*]BookConsumerService.Entities.*%2c[*]BookConsumerService.Infrastructure.*%2c[*]BookConsumerService.Models.*  /p:MergeWith=../CoverageResults/coverage.json /p:CoverletOutputFormat=opencover%2cjson -m:1

dotnet tool install -g dotnet-reportgenerator-globaltool //run once
reportgenerator -reports:.\CoverageResults\coverage.opencover.xml -targetdir:.\CoverageResults