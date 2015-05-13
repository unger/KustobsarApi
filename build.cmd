@echo Off

set solutionName=KustobsarApi.sln
set MSBuildExe="C:\Program Files (x86)\MSBuild\12.0\Bin\msbuild.exe"

set config=%1
if "%config%" == "" (
	set config=Release
)

set buildDir=Build\%config%


nuget restore
%MSBuildExe% %solutionName% /t:Rebuild /p:Configuration=%config%;OutDir="%cd%\%buildDir%";UseWPP_CopyWebApplication=True;PipelineDependsOnBuild=False

