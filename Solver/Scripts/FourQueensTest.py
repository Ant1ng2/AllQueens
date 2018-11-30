from FourQueens import *
import random

row = []
positions = []

randomSamples = random.sample(range(25), 12)
whiteSamples = randomSamples[:6]
blackSamples = randomSamples[6:]

list = []
for i in range(25):
    if i in whiteSamples:
        list += ["white"]
    else:
        if i in blackSamples:
            list += ["black"]
        else:
            list += ["     "]

for element in list:
    row += [element]
    if len(row) == 5:
        positions += [row]
        row = []

print("Random board")
for row in positions:
    print(["     " if i == "" else i for i in row])

print(encode(FourQueens(positions)))
