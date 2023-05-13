sonarqube-PR:
	dotnet sonarscanner begin /k:"nibss-dev_NIBSS-Website" /o:"nibss-dev" \
		/d:sonar.host.url="https://sonarcloud.io" /d:sonar.login=${SONAR_TOKEN} \
		/d:sonar.pullrequest.key=${CHANGE_ID} \
		/d:sonar.exclusions="./**/*.cshtml" \
		/d:sonar.cs.opencover.reportsPaths="./coverage/coverage.opencover.xml"

	dotnet build nibss_orchad_azure.sln
	dotnet sonarscanner end /d:sonar.login=${SONAR_TOKEN}

sonarqube-BR:
	dotnet sonarscanner begin /k:"nibss-dev_NIBSS-Website" /o:"nibss-dev" \
		/d:sonar.host.url="https://sonarcloud.io" /d:sonar.login=${SONAR_TOKEN} \
		/d:sonar.exclusions="./**/**/*.cshtml" \
		/d:sonar.branch.name=${BRANCH_NAME} \
		/d:sonar.cs.opencover.reportsPaths="./coverage/coverage.opencover.xml"

	dotnet build nibss_orchad_azure.sln
	dotnet sonarscanner end /d:sonar.login=${SONAR_TOKEN}

test:
	dotnet test "CodeCoverage/CodeCoverage.csproj" \
		/p:CollectCoverage=true \
		/p:CoverletOutputFormat=\"json,lcov,opencover\" \
		/p:CoverletOutput=\"../coverage/\" \
	    /p:Exclude=\"[*.Views?]*\" \
		/p:MergeWith=\"../coverage/coverage.json\"
