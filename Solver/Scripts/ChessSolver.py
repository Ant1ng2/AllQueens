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
        serialized = game.serialize()
        if serialized in self.memory:
            return self.memory[serialized]
        primitive = game.primitive()
        if primitive != Value.Undecided:
            self.memory[serialized] = primitive
            return primitive
        for move in game.generateMoves():
            newFourQueens = game.doMove(move)
            if self.solve(newFourQueens) == FourQueens.Value.Lose:
                self.memory[serialized] = FourQueens.Value.Win
                return FourQueens.Value.Win # Not necessarily traverse all subtree
            self.memory[serialized] = FourQueens.Value.Lose
        return FourQueens.Value.Lose

	# this one will traverse all subtree
    def solveTraverse(self, game):
        winFlag = False
        serialized = game.serialize()
        if serialized in self.memory:
            return self.memory[serialized]
        primitive = game.primitive()
        if primitive != Value.Undecided:
            self.memory[serialized] = primitive
            return primitive
        for move in game.generateMoves():
            newFourQueens = game.doMove(move)
            if self.solve(newFourQueens) == FourQueens.Value.Lose:
                self.memory[serialized] = FourQueens.Value.Win
                winFlag = True
            if not winFlag:
                self.memory[serialized] = FourQueens.Value.Lose
        return FourQueens.Value.Win if winFlag else FourQueens.Value.Lose

    # Can be executed through precomputation or computation per move
    def generateMove(self, game):
        if self.solve(game) == FourQueens.Value.Lose:
            return game.generateMoves()[0]
        validMoves = [
            move for move in game.generateMoves() if self.solve(game.doMove(move) == FourQueens.Value.Lose)
        ]
        return validMoves[0]
