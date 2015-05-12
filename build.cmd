@echo Off


set solutionName=KustobsarApi.sln
set config=Release
set buildDir=Build


nuget restore
C:\Windows\Microsoft.Net\Framework64\v4.0.30319\msbuild.exe %solutionName% /p:Configuration=%config% /p:OutDir=%cd%\%buildDir% /p:UseWPP_CopyWebApplication=True /p:PipelineDependsOnBuild=False