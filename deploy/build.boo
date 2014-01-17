solution_name = "MyCouch-AspNet-Identity"
solution_dir_path = "../src"
project_name = "MyCouch.AspNet.Identity"
builds_dir_path = "builds"
build_version = "0.1.0"
build_config = "Release"
build_name = "${project_name}-v${build_version}-${build_config}"
build_dir_path = "${builds_dir_path}/${build_name}"
nuget = "nuget.exe"

target default, (clean, compile, copy, zip, nuget_pack):
    pass

target copy, (copy_core):
	pass

target nuget_pack, (nuget_pack_core):
	pass

target clean:
    rm(build_dir_path)

target compile:
    msbuild(
        file: "${solution_dir_path}/${solution_name}.sln",
        targets: ("Clean", "Build"),
        configuration: build_config)

target copy_core:
    with FileList("${solution_dir_path}/Projects/${project_name}.Net45/bin/${build_config}"):
        .Include("${project_name}.*.{dll,xml}")
        .ForEach def(file):
            file.CopyToDirectory("${build_dir_path}/Net45")

target zip:
    zip(build_dir_path, "${builds_dir_path}/${build_name}.zip")

target nuget_pack_core:
    exec(nuget, "pack ${project_name}.nuspec -version ${build_version} -basepath ${build_dir_path} -outputdirectory ${builds_dir_path}")