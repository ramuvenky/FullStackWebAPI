..\packages\OpenCover.4.6.519\tools\opencover.console.exe -register:user "-filter:+[FullStackWebAPI]*" "-target:..\packages\NUnit.ConsoleRunner.3.10.0\tools\nunit3-console.exe" "-targetargs:.\bin\Debug\FullStackWebAPI.Tests.dll"

..\packages\ReportGenerator.3.1.2\tools\ReportGenerator.exe "-reports:results.xml" "-targetdir:.\coverage"

pause