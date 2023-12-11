
class Reel {

    #reelNumber;

    // Refs
    #symbolsContainer;
    #originalSymbolNodes;

    // Props
    #stopSymbolIndex;
    #stopSignal = false;
    #nextSymbolIndex = 0;
    #stepRefreshRateMs = 100;
    #playlineOffset = 4;

    // Util
    #promiseResolveHandler;

    constructor(reelNumber) {
        this.#reelNumber = reelNumber;
        this.#symbolsContainer = document.querySelector(`.reel[data-reel='${reelNumber}'] .symbols-container`);
        this.#originalSymbolNodes = this.#getActualReelSymbols();
        this.#nextSymbolIndex = this.#originalSymbolNodes.length - 1;
    }

    spinReelWithResult(resultSymbol, spinDuration) {
        return new Promise((resolve) => {
            this.#promiseResolveHandler = resolve;

            this.#stopSymbolIndex = resultSymbol;
            setTimeout(this.#spinStep.bind(this), this.#stepRefreshRateMs);

            this.#stopSignal = false;
            setTimeout(() => this.#stopSignal = true, spinDuration);
        });
    }

    displayWin() {
        this.#getActualReelSymbols()[this.#playlineOffset - 1].classList.add("scale-up-center");
    }

    stopWinDisplay() {
        this.#getActualReelSymbols().forEach(i => i.classList.remove("scale-up-center"));
    }

    #getActualReelSymbols() {
        return this.#symbolsContainer.querySelectorAll(".reel-symbol");
    }

    #spinStep() {
        const newSymbol = this.#originalSymbolNodes[this.#nextSymbolIndex];
        this.#nextSymbolIndex--;
        if (this.#nextSymbolIndex === -1)
            this.#nextSymbolIndex = this.#originalSymbolNodes.length - 1;

        newSymbol.classList.remove("show");
        this.#symbolsContainer.prepend(newSymbol);
        
        setTimeout(() => {
            newSymbol.classList.add("show");
        }, 100);

        let currentSymbolIndex = this.#loopIndex(this.#nextSymbolIndex, this.#playlineOffset, this.#originalSymbolNodes.length);
        
        if (this.#stopSignal && currentSymbolIndex === this.#stopSymbolIndex) {
            this.#promiseResolveHandler();
            return;
        }

        setTimeout(this.#spinStep.bind(this), this.#stepRefreshRateMs);
    }

    #loopIndex(index, plusValue, maxBound) {
        const newValue = index + plusValue;
        return Math.abs(newValue % maxBound);
    }
}

// Proxy for Blazor
export function createReel(reelNumber) {
    return new Reel(reelNumber);
}