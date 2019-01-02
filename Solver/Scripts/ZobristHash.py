import random

class ZobristHash:

    def __init__(self):
        self.table = []
        for _ in range(25):
            row = []
            for _ in range(2):
                row.append(random.getrandbits(32))
            self.table += [row]

    def hash(self, board, code=None, start=None, end=None):
        indices = {
            "w" : 0,
            "b" : 1,
        }
        if code and start and end:
            j = board[start[0]][start[1]]
            if not j:
                return code
            temp = code ^ self.table[start[1]*5 + start[0]][indices[j]]
            temp = temp ^ self.table[end[1]*5 + end[0]][indices[j]]
            return temp
        h = 0
        for i in range(25):
            j = board[i%5][i//5]
            if j:
                h = h ^ self.table[i][indices[j]]
        return h
