#!/usr/bin/env python
# coding=utf-8
from functools import lru_cache
import os
from flask import Blueprint, request
from rez.resolved_context import ResolvedContext
from results import Results

rez_env_api = Blueprint("rez_env", __name__)


@rez_env_api.route("/env", methods=["POST"])
def get_rez_env():
    data = request.get_json()
    pkg_str = data.get("rez_str")
    if pkg_str:
        try:
            res_rez = resolved_context(pkg_str)
            return Results(data=res_rez).result()
        except Exception as e:
            return Results(msg="{}".format(e), status=False).result()
    else:
        return Results(msg="error", status=False).result()


@lru_cache(maxsize=20)
def resolved_context(pkg: str) -> dict:
    print(pkg)
    if " -- " in pkg:
        pkg = pkg.split(" -- ")[0]

    pkg_list = pkg.split(" ")
    if pkg_list[0] == "env":
        del pkg_list[0]

    res_rez = ResolvedContext(pkg_list)
    rez_env_dict = res_rez.get_environ()
    if rez_env_dict.get("PATH"):
        rez_env_dict["PATH"] = clean_path_env(rez_env_dict["PATH"])
    env_key = [key_name for key_name in rez_env_dict]

    for item in env_key:
        if item.startswith("REZ_"):
            del rez_env_dict[item]
        rez_env_dict[item] = rez_env_dict[item].replace("\\", "/")

    return rez_env_dict


def clean_path_env(path_env: str) -> str:
    new_path_env = []
    current_path_env_list = os.environ.get("PATH").replace("\\", "/").split(os.pathsep)
    rez_env_path_list = path_env.replace("\\", "/").split(os.pathsep)
    for item in rez_env_path_list:
        if item not in current_path_env_list:
            new_path_env.append(item)

    return os.pathsep.join(new_path_env)
