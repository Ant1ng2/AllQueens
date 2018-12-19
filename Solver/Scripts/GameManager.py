from FourQueens import *
class GameManger:

    def __init__(self, game):
        self.game = game

    def play(self):
        while self.game.primitive() != "Undecided":
            print(self.game.getTurn(), "'s turn")
            print(self.game)
            print("Enter Piece: ")
            start = tuple(int(x.strip()) for x in input().split(','))
            print("Move Piece where: ")
            finish = tuple(int(x.strip()) for x in input().split(','))

            self.game.doMove((start, finish))
            print("----------------------------")


game = FourQueens()
gameManager = GameManger(game)
gameManager.play()
