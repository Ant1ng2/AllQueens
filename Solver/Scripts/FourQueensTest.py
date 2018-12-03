from FourQueens import *
import random

row = []
positions = []

print("Initialized Board")

randomSamples = random.sample(range(25), 12)
whiteSamples = randomSamples[:6]
blackSamples = randomSamples[6:]

list = []
for i in range(25):
    if i in whiteSamples:
        list += ["w"]
    else:
        if i in blackSamples:
            list += ["b"]
        else:
            list += [""]

for element in list:
    row += [element]
    if len(row) == 5:
        positions += [row]
        row = []

print("Random board")
print(FourQueens(positions))

print(encode(FourQueens(positions)))
