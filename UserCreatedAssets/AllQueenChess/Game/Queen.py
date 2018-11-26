class Queen():
    #Creating a new queen
    def __init__(self):
        self.QueenDirections = [
            (0,1), (1,0), (0,-1), (-1,0),
            (1,1), (1,-1), (-1,-1), (-1,1)
        ]

    def getMoves(self, game, position):
        locations = []
        for direction in self.QueenDirections:
            for x in range(4):
                nextGridPoint = (
                    position[0] + (x + 1) * direction[0],
                    position[1] + (x + 1) * direction[1]
                )
                #Reference Point #1: Add a check for a piece
                if (
                    nextGridPoint[0] < 0 or nextGridPoint[0] > 4 or
                    nextGridPoint[1] < 0 or nextGridPoint[1] > 4 or
                    game[nextGridPoint[0]][nextGridPoint[1]]
                ):
                    break
                locations = locations + [nextGridPoint]
        return locations
