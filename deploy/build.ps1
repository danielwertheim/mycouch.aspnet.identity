Framework "4.5.1"

Properties {
    $solution_name = "MyCouch-AspNet-Identity"
    $solution_dir_path = "..\src"
    $solution_path = "$solution_dir_path\$solution_name.sln"
    $project_name = "MyCouch.AspNet.Identity"
    $builds_dir_path = "builds"
    $build_version = "0.2.0"
    $build_config = "Release"
    $build_name = "${project_name}-v${build_version}-${build_config}"
    $build_dir_path = "${builds_dir_path}\${build_name}"
    $nuget = "nuget.exe"
}

task default -depends Clean, Build, Copy, Nuget-Pack

task Clean {
    Clean-Directory("$build_dir_path")
}

task Build {
    Exec { msbuild "$solution_path" /t:Clean /v:quiet }
    Exec { msbuild "$solution_path" /t:Build /p:Configuration=$build_config /v:quiet }
}

task Copy {
    CopyTo-Build("$project_name.Net45")
}

task NuGet-Pack {
    NuGet-Pack-Project($project_name)
}

Function NuGet-Pack-Project($t) {
    & $nuget pack "$t.nuspec" -version $build_version -basepath $build_dir_path -outputdirectory $builds_dir_path
}

Function EnsureClean-Directory($dir) {
    Clean-Directory($dir)
    Create-Directory($dir)
}

Function Clean-Directory($dir){
	if (Test-Path -path $dir) {
        rmdir $dir -recurse -force
    }
}

Function Create-Directory($dir){
	if (!(Test-Path -path $dir)) {
        new-item $dir -force -type directory
    }
}

Function CopyTo-Build($t) {
    $trg = "$build_dir_path\$t"
    EnsureClean-Directory($trg)
    
    CopyTo-Directory "$solution_dir_path\projects\$t\bin\$build_config\$t.*" $trg
}

Function CopyTo-Directory($src, $trg) {
    Copy-Item -Path $src -Include *.dll,*.xml -Destination $trg -Recurse -Container
}