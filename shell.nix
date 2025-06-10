{ pkgs ? import <nixpkgs> {} }:

pkgs.mkShell rec {
  dotnetPkg = pkgs.dotnet-sdk_8;

  runtimeDeps = [
    pkgs.fontconfig
    pkgs.xorg.libX11
    pkgs.xorg.libICE
    pkgs.xorg.libSM
  ];

  buildInputs = [
    dotnetPkg
  ] ++ runtimeDeps;

  LD_LIBRARY_PATH = pkgs.lib.makeLibraryPath runtimeDeps;

  DOTNET_ROOT = "${dotnetPkg}";
}

