#!/usr/bin/env python
# coding=utf-8


class Results:
    def __init__(self, data=None, msg="success", status=True):
        if data is None:
            data = {}
        self.msg = msg
        self.data = data
        self.status = status

    def result(self) -> dict:
        return {"msg": self.msg, "status": self.status, "data": self.data}

