from Queen import Queen
from enum import Enum
import math

lineDirections = [(0,1), (1,0), (1,1), (1,-1)]

class Value(Enum):
	Win = "Win"
	Lose = "Lose"
	Undecided = "Undecided"

class FourQueens():

	def __init__(self, state=None, turn="w"):
		if state is None:
			self.pieces = []
			for _ in range(5):
				row = []
				for _ in range(5):
					row = row + [""]
				self.pieces = self.pieces + [row]

			self.addPiece("b", (0, 0))
			self.addPiece("b", (0, 2))
			self.addPiece("b", (2, 0))
			self.addPiece("b", (4, 0))
			self.addPiece("b", (1, 4))
			self.addPiece("b", (3, 4))

			self.addPiece("w", (4, 4))
			self.addPiece("w", (4, 2))
			self.addPiece("w", (2, 4))
			self.addPiece("w", (0, 4))
			self.addPiece("w", (1, 0))
			self.addPiece("w", (3, 0))

			self.turn = turn
			self.winner = ""
		else:
			self.pieces = state
			self.turn = turn
			self.winner = ""
			self.primitive()

	def addPiece(self, player, position):
		self.pieces[position[0]][position[1]] = player

	def __str__(self):
		boardStr = ''
		for row in self.pieces:
			for piece in row:
				if not piece:
					boardStr += "-"
				else:
					boardStr += piece
			boardStr += "\n"
		return boardStr

	def generateDictionaryMoves(self):
		i = 0
		dict = {}
		queen = Queen()
		while i < 25:
			piece = self.pieces[i%5][i//5]
			if piece == self.turn:
				dict[(i%5, i//5)] = (
					piece,
					queen.getMoves(self.pieces, (i%5, i//5))
				)
			else:
				dict[(i%5, i//5)] = ()
			i+=1
		return dict

	def generateMoves(self):
		i = 0
		list = []
		queen = Queen()
		while i < 25:
			piece = self.pieces[i%5][i//5]
			if piece == self.turn:
				for move in queen.getMoves(self.pieces, (i%5, i//5)):
					list = list + [((i%5, i//5), move)]
			i+=1
		return list

	def doMove(self, move):
		start = move[0]
		end = move[1]
		temp = self.pieces[start[0]][start[1]]
		self.pieces[start[0]][start[1]] = ""
		self.pieces[end[0]][end[1]] = temp

		if self.checkWin(end):
			print(self.turn, " wins")

		if self.turn == "w":
			self.turn = "b"
		else:
			self.turn = "w"

	def checkWin(self, end):
		"""Note: Only checks if someone has won on a specific position
		AND has made the last move.
		"""
		for direction in lineDirections:
			i = -3
			lineLen = 0
			while i < 4:
				x = direction[0] * i + end[0]
				y = direction[1] * i + end[1]
				if (x >= 0 and y >= 0 and
					x < 5 and y < 5):
					piece = self.pieces[x][y]
					if piece and piece == self.turn:
						lineLen += 1
						if lineLen >= 4:
							self.winner == self.turn
							return True
					else:
						lineLen = 0
				i += 1
		return False

	def primitive(self):
		if self.winner:
			if self.winner == self.turn:
				return Value.Win
			else:
				return Value.Lose

		"""
		for i in range(5):
			for j in range(5):
				if self.checkWin((i, j)):
					if self.pieces[i][j] == self.turn:
						return Value.Win
					else:
						return Value.Lose
		"""
		return Value.Undecided

	def serialize(self):
		return

def encode(game, pieces=None, turn=None):
	if turn is None:
		turn = game.turn

	if pieces is None:
		pieces = game.pieces

	list = []
	#Encodes the position
	for i in range(5):
		for j in range(5):
			piece = pieces[i][j]
			list = list + [piece]

	#Combinatorial Number System for spaces and queens
	k = 1
	spaces = 0
	for i, j in enumerate(list):
		if j:
			spaces += combine(i, k)
		k += 1

	#Combinatorial Number System for queen orientations
	number = 0
	order = [j for j in list if j]
	indices = [i for i, j in enumerate(order) if j == turn]
	for i, j in enumerate(indices):
		number += combine(j, i+1)

	return space << 10 + number

def decode(mes):
	number = mes % 0b1111111111
	spaces = mes >> 10

	order = []
	k = 6
	while k > 0:
		n = 0
		while (combine(n, k) <= number):
			n += 1
		n -= 1
		c = combine(n, k)
		number = number - c
		order = [n] + order
		k -= 1

	list = []
	k = 12
	while k > 0:
		n = 0
		while (combine(n, k) <= spaces):
			n += 1
		n -= 1
		c = combine(n, k)
		spaces = spaces - c
		list = [n] + list
		k -= 1

	positions = []
	row = []
	for i, j in enumerate(list):
		if (i%5 == 0):
			positions = positions + [row]
			row = []
		if j and order:
			row = row + [order[0]]
			order = order[1:]
		else:
			row = row + [""]

	return positions

def combine(n, k):
	return math.factorial(n)/(math.factorial(n-k)*math.factorial(k))
