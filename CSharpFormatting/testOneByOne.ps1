foreach ($line in (dotnet test -t 2> Out-Null))
{
    if ($line.StartsWith("    "))
    {
        $fqn = $line.SubString(4)
        $projectName = ""
        foreach ($fqnPart in $fqn.Split('.'))
        {
            if ($projectName -eq "")
            {
                $projectName = "$fqnPart"
            }
            else 
            {
                $projectName = "$projectName.$fqnPart"
            }

            if ($fqnPart -eq "Test")
            {
                break;
            }
        }

        $fqn

        $runAndProcessMeasurement = Start-Job -ArgumentList (Get-Location),$fqn,$projectName -ScriptBlock {
            param($path, $fqn, $projectName)

            if (-not $projectName.Contains("IntegrationTest")) {
                return
            }

            Set-Location $path

            .\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:"test --filter `"FullyQualifiedName=$fqn`" --logger:trx;LogFileName=results.trx /p:DebugType=full $projectName/$projectName.csproj" -filter:"+[*]*" -output:opencoveroutput.xml -oldstyle | Out-Null

            [xml]$results = gc -Path opencoveroutput.xml
            foreach ($module in $results.CoverageSession.Modules.Module)
            {
                if ($module.Summary -and -not $module.ModuleName.Contains("Test"))
                {
                    "    $($module.ModuleName) $($module.Summary.visitedSequencePoints)/$($module.Summary.numSequencePoints)"
                }
            }
        }

        if (Wait-Job $runAndProcessMeasurement -Timeout 30) { Receive-Job $runAndProcessMeasurement }
        Remove-Job -Force $runAndProcessMeasurement
    }
}