#! /bin/bash

echo -e "load env"

{{rez_env_body}}

konsole --hide-menubar --hide-tabbar -e {{run_app}} >/dev/null 2>&1
