#!/usr/bin/env python
# coding=utf-8
import subprocess
from functools import lru_cache
from flask import Blueprint, request
from results import Results

rez_res_api = Blueprint("rez_search", __name__)


@rez_res_api.route("/implicit", methods=["GET"])
def get_implicit_env():
    args = request.args
    pkg_str = args.get("rez_str")
    if pkg_str:
        try:
            res_rez = implicit_app(pkg_str)
            return Results(data=res_rez).result()
        except Exception as e:
            return Results(msg="{}".format(e), status=False).result()
    else:
        return Results(msg="error", status=False).result()


@lru_cache(maxsize=5)
def implicit_app(pkg_str: str) -> list:
    tools = []
    sub = subprocess.check_output('rez search %s -f="{requires}"' % pkg_str).decode("utf-8").split(" ")
    for item in sub:
        if item.startswith("~"):
            tools.append(item[1:-1])
    return tools

