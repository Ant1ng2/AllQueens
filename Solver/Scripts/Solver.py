from FourQueens import *
from Queen import *

class Solver():

	def __init__(self):
		self.memory = {}

	def resetMemory(self):
		self.memory.clear()

	def solveWeakWithoutMemory(self, game):
		primitive = game.primitive()
		if primitive != FourQueens.Value.Undecided:
			return primitive
		for move in game.generateMoves():
			newFourQueens = game.doMove(move)
			if self.solve(newFourQueens) == FourQueens.Value.Lose:
				return FourQueens.Value.Win # Not necessarily traverse all subtree
		return FourQueens.Value.Lose

	# this one will end when it finds the next instance as Win
	def solve(self, game):
		serialized = game.serialize()
		if serialized in self.memory:
			return self.memory[serialized]
		primitive = game.primitive()
		if primitive != FourQueens.Value.Undecided:
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
		if primitive != FourQueens.Value.Undecided:
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

solver = Solver()
game = FourQueens()
print(solver.solveTraverse(game))

memory = []
for game, value in solver.memory.items():
	memory.append((game, value))

memory.sort(key=lambda item: int(item[0].split()[0]), reverse=True)
for item in memory:
	print(item[0], item[1].name)
