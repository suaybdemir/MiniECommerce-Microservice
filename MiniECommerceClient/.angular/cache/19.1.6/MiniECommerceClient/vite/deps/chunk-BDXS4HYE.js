import {
  Pipe,
  setClassMetadata,
  ɵɵdefinePipe
} from "./chunk-OAR43WPE.js";

// node_modules/tr-currency/fesm2022/tr-currency.mjs
var TrCurrencyPipe = class _TrCurrencyPipe {
  transform(value, symbol = "", isCurrencyFront = true, fraction = 2) {
    if (fraction < 0) fraction = 0;
    if (value === void 0 || value === null || isNaN(Number(value))) {
      if (fraction === 0) {
        return isCurrencyFront ? `${symbol}0` : `0 ${symbol}`;
      } else {
        return isCurrencyFront ? `${symbol}0,${"0".repeat(fraction)}` : `0,${"0".repeat(fraction)} ${symbol}`;
      }
    }
    value = Number(value);
    let isValueNegative = false;
    if (value < 0) {
      isValueNegative = true;
      value = Math.abs(value);
    }
    value = parseFloat(value.toFixed(fraction));
    let money = value.toString().split(".");
    let newMoney = "";
    let lira = money[0];
    let penny = money.length > 1 ? money[1] : "";
    if (penny.length < fraction) {
      penny += "0".repeat(fraction - penny.length);
    }
    let count = 0;
    for (let i = lira.length; i > 0; i--) {
      if (count === 3 && count < lira.length) {
        newMoney = lira[i - 1] + "." + newMoney;
        count = 1;
      } else {
        newMoney = lira[i - 1] + newMoney;
        count++;
      }
    }
    if (!isCurrencyFront) {
      newMoney = fraction === 0 ? `${newMoney} ${symbol}` : `${newMoney},${penny} ${symbol}`;
    } else {
      newMoney = fraction === 0 ? `${symbol}${newMoney}` : `${symbol}${newMoney},${penny}`;
    }
    if (isValueNegative) {
      newMoney = `-${newMoney}`;
    }
    return newMoney;
  }
  static {
    this.ɵfac = function TrCurrencyPipe_Factory(__ngFactoryType__) {
      return new (__ngFactoryType__ || _TrCurrencyPipe)();
    };
  }
  static {
    this.ɵpipe = ɵɵdefinePipe({
      name: "trCurrency",
      type: _TrCurrencyPipe,
      pure: true
    });
  }
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(TrCurrencyPipe, [{
    type: Pipe,
    args: [{
      name: "trCurrency",
      standalone: true
    }]
  }], null, null);
})();

export {
  TrCurrencyPipe
};
//# sourceMappingURL=chunk-BDXS4HYE.js.map
