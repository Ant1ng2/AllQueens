from FourQueens import *

class Solver():

    def __init__(self):
        self.memory = {}

    def resetMemory(self):
        self.memory.clear()

    def solve(self, game):
        hash = game.serialize()
        if hash in self.memory:
            return
        primitive = game.primitive()
        if primitive != FourQueens.Value.Undecided:
            self.memory[serialized] = primitive
            return primitive
        for move in game.generateMoves():
            game.doMove(move)
            self.solve(game)


    def generateMove(self, game):
