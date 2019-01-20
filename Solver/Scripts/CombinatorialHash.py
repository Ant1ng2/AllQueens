class CombinatorialHash:

    def Choose(n, k):
        if k > n:
            return 0
        if k * 2 > n:
            k = n - k
        if k == 0:
            return 1

        result = n
        for i in range(2, k+1):
            result *= (n - i + 1)
            result /= i
        return result

    def hash(board, turn):
        hashPieces = 0
        hashColors = 0

        l = 12
        k = 6

        for i in range(25):
            j = board.getPiece(i % 5, i / 5)
            if j and l > 0:
                hashPieces += Choose(24 - i, l)
                l-=1
                if j == turn:
                    hashColors += Choose(l, k)
                    k-=1

        return hashColors << 32 | hashPieces

    def unhash(hash, current, other):
        hashColors = hash >> 32
        hashPieces = hash & (2**32 - 1)

		pieces = []
		for _ in range(5):
			row = []
			for _ in range(5):
				row = row + [""]
			pieces = pieces + [row]

        l = 12
        k = 6
        f = 6

        for i in range(25):
            value = Choose(24 - i, l)
            if hashPieces >= value:
                hashPieces -= value
                l -= 1
                value = Choose(l, k)
                if hashColors >= value
                    hashColors -= value
                    k -= 1
                    pieces[i % 5, i / 5] = current
                else:
                    f -= 1
                    pieces[i % 5, i / 5] = other

        return pieces
