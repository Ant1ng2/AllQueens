from Queen import Queen
from enum import Enum
import math
from ZobristHash import *

lineDirections = [(0,1), (1,0), (1,1), (1,-1)]

class Value(Enum):
	Win = "Win"
	Lose = "Lose"
	Undecided = "Undecided"

def main():
	game = FourQueens()

class FourQueens():
	def __init__(self, state=None, turn="w", hash=ZobristHash(), winner=""):
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
		else:
			self.pieces = state

		self.turn = turn
		self.winner = winner
		self.hash = hash

	def addPiece(self, player, position):
		self.pieces[position[0]][position[1]] = player

	def getPiece(self, position):
		return self.pieces[position[0]][position[1]]

	def getTurn(self):
		return self.turn

	def __str__(self):
		pieces = rotate(rotate(rotate(self.pieces)))
		boardStr = ''
		for row in pieces:
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
		winner = ""
		pieces = [[piece for piece in row] for row in self.pieces]
		temp = pieces[start[0]][start[1]]
		pieces[start[0]][start[1]] = ""
		pieces[end[0]][end[1]] = temp

		if self.checkWin(pieces, end):
			winner = self.turn

		if self.turn == "w":
			turn = "b"
		else:
			turn = "w"

		return FourQueens(pieces, turn, self.hash, winner)

	def checkWin(self, pieces, end):
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
					piece = pieces[x][y]
					if piece and piece == self.turn:
						lineLen += 1
						if lineLen >= 4:
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
		return Value.Undecided

	def serialize(self):
		max = 0
		temp = self.pieces

		for _ in range(2):
			for _ in range(4):
				if max < value(temp):
					max = value(temp)
					pieces = temp
				temp = rotate(temp)
			temp = flip(temp)
		return self.hash.hash(pieces)

def rotate(board):
	pieces = []
	for i in list(zip(*reversed(board))):
		pieces += [list(i)]
	return pieces

def flip(board):
	for i in board:
		pieces = [i] + pieces
	return pieces

def value(board):
	total = 0
	i = 0
	for row in board:
		for element in row:
			if element:
				total += 2**i
			i+=1
	return total

if __name__ == "__main__":
	main()
