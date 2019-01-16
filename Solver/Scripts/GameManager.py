from FourQueens import *
import ChessSolver

class GameManger:

    def __init__(self, game, solver=None):
        self.game = game
        self.solver = solver
        #if solver:
            #self.solver.solveTraverse(self.game)

    def play(self):
        while self.game.primitive() == Value.Undecided:
            print(self.game.getTurn(), "'s turn")
            print(self.game)
            if self.game.getTurn() == "w" or not self.solver:
                print("Enter Piece: ")
                start = tuple(int(x.strip()) for x in input().split(','))
                print("Move Piece where: ")
                finish = tuple(int(x.strip()) for x in input().split(','))
                if (start, finish) not in self.game.generateMoves():
                    print("Not a valid move, try again")
                else:
                    self.game = self.game.doMove((start, finish))
                print("----------------------------")
            else:
                self.game = self.game.doMove(self.solver.generateMove(self.game))
        print(self.game)
        print("Game Over")

game = FourQueens()
gameManager = GameManger(game, ChessSolver.Solver())
gameManager.play()
