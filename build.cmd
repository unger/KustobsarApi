@echo Off

set solutionName=
for %%i in (*.sln) do set solutionName=%%i

if "%solutionName%" == "" (
	Echo No Solution file found in folder
	Exit /b
)

set config=%1
if "%config%" == "" (
	set config=Release
)

for /f "tokens=1-3 delims=-" %%a in ("%DATE%") do (set mydate=%%a%%b%%c)
for /f "tokens=1-2 delims=/: " %%a in ("%TIME%") do (set mytime=0%%a%%b)
SET mytime=%mytime:~-4%

set buildTempDir=Build\Temp
set buildOutDir=Build\%config%\%mydate%_%mytime%


nuget restore
msbuild %solutionName% /t:Rebuild /p:Configuration=%config%;OutDir="%cd%\%buildTempDir%";UseWPP_CopyWebApplication=True;PipelineDependsOnBuild=False

set webProjectOutputFolder=
for /d %%i in (%buildTempDir%\_PublishedWebsites\*) do set webProjectOutputFolder=%%i

if not "%webProjectOutputFolder%" == "" (
	robocopy %webProjectOutputFolder% %buildOutDir%\Site /MIR /R:5 /W:1 /FFT

	REM Zip output folder
	powershell.exe -nologo -noprofile -command "& { Add-Type -A 'System.IO.Compression.FileSystem'; [IO.Compression.ZipFile]::CreateFromDirectory('%buildOutDir%\Site', '%buildOutDir%.zip'); }"
)

