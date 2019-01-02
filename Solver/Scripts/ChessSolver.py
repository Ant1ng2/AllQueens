from FourQueens import *

class Solver():

    def __init__(self):
        self.memory = {}

    def resetMemory(self):
        self.memory.clear()

    # Thanks to GameCrafters member Seichan for producing the following
    # two functions.

    # this one will end when it finds the next instance as Win
    def solve(self, game):
        print(game)
        serialized = game.serialize()
        if serialized in self.memory:
            return self.memory[serialized]
        primitive = game.primitive()
        self.memory[serialized] = primitive
        if primitive != Value.Undecided:
            return primitive
        for move in game.generateMoves():
            newFourQueens = game.doMove(move)
            if self.solve(newFourQueens) == Value.Lose:
                self.memory[serialized] = Value.Win
                return Value.Win # Not necessarily traverse all subtree
            self.memory[serialized] = Value.Lose
        return Value.Lose

	# this one will traverse all subtree
    def solveTraverse(self, game):
        winFlag = False
        serialized = game.serialize()
        if serialized in self.memory:
            return self.memory[serialized]
        primitive = game.primitive()
        self.memory[serialized] = primitive
        if primitive != Value.Undecided:
            return primitive
        for move in game.generateMoves():
            newFourQueens = game.doMove(move)
            if self.solve(newFourQueens) == Value.Lose:
                self.memory[serialized] = Value.Win
                winFlag = True
        if not winFlag:
            self.memory[serialized] = Value.Lose
        return Value.Win if winFlag else Value.Lose

    # Can be executed through precomputation or computation per move
    def generateMove(self, game):
        if self.solve(game) == Value.Lose:
            return game.generateMoves()[0]
        validMoves = [
            move for move in game.generateMoves() if self.solve(game.doMove(move) == Value.Lose)
        ]
        return validMoves[0]
