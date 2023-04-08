@ECHO OFF

:: rez env

{{rez_env_body}}


start /B "App" {{run_app}}
