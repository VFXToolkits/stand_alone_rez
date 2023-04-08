#!/usr/bin/env python
# coding=utf-8
import sys
from flask import Flask
from api.rez_env import rez_env_api
from api.rez_search import rez_res_api


def create_app() -> Flask:
    app = Flask(__name__)

    app.register_blueprint(rez_env_api)
    app.register_blueprint(rez_res_api)
    return app


if "__main__" == __name__:
    server_app = create_app()
    if len(sys.argv) == 3:
        server_app.run(host=sys.argv[-2], port=int(sys.argv[-1]))
    else:
        server_app.run(host="127.0.0.1", port=2531)


