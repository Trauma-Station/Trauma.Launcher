{
  description = "Flake providing a package for the Space Station 15 Launcher.";

  inputs.nixpkgs.url = "github:NixOS/nixpkgs/nixpkgs-unstable";

  outputs =
    { self, nixpkgs, ... }:
    let
      forAllSystems =
        function:
        nixpkgs.lib.genAttrs [ "x86_64-linux" ] # TODO: aarch64-linux support
          (system: function system (import nixpkgs { inherit system; }));
    in
    {

      packages = forAllSystems (
        system: pkgs: {
          default = self.packages.${system}.trauma-station-launcher;
          trauma-station-launcher = pkgs.callPackage ./nix/package.nix { };
        }
      );

      overlays = {
        default = self.overlays.trauma-station-launcher;
        trauma-station-launcher = final: prev: {
          trauma-station-launcher =
            self.packages.${prev.stdenv.hostPlatform.system}.trauma-station-launcher;
        };
      };

      apps = forAllSystems (
        system: pkgs:
        let
          pkg = self.packages.${system}.trauma-station-launcher;
        in
        {
          default = self.apps.${system}.trauma-station-launcher;
          trauma-station-launcher = {
            type = "app";
            program = "${pkg}/bin/${pkg.meta.mainProgram}";
          };
          fetch-deps = {
            type = "app";
            program = toString self.packages.${system}.trauma-station-launcher.passthru.fetch-deps;
          };
        }
      );

      formatter = forAllSystems (_: pkgs: pkgs.nixpkgs-fmt);

    };
}
