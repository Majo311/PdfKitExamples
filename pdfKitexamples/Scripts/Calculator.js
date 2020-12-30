var result = this.getField('result');
var operator = this.getField('operator');
var accum;      //The accumulator to store intermidiate results
var entry;      //The active entry, so we do not have to parse the result.Value.
var multiplier; //The active multiplier (can be 1 or 10)
var divisor;    //The active divisor (can be 1, 10, 100, ... ), needed for DOT support.
clear_all();
// Function to clear all (C)
function clear_all() {
    clear_entry()
    result.value = 0;
    accum = 0;
    operator.value = '';
}
// Function to clear current entry only (CE)
function clear_entry()
{
    entry = 0;
    multiplier = 1;
    divisor = 1;
} 
function operator_pressed(op) {
    if (operator.value != '') {
        update_result();
    }
    else {
        if (entry != 0) {
            accum = entry;
        }
    }
    operator.value = op;
    clear_entry();
} 
 //Number pressed
function number_pressed(number) {
    entry = entry * multiplier + number / divisor;
    if (divisor >= 10) {
        divisor = divisor * 10;
    }
    else {
        multiplier = 10;
    }
    result.value = entry;
} 
// Function to update result
function update_result() {
    switch (operator.value) {
        case 'PLUS':
            accum = accum + entry;
            break;
        case 'MINUS':
            accum = accum - entry;
            break;
        case 'MULTIPLY':
            accum = accum * entry;
            break;
        case 'DIVIDE':
            if (entry != 0) {
                accum = accum / entry;
            }
            break;
    }
    operator.value = '';
    clear_entry();
}

function dot_pressed() {
    if (divisor == 1) {
        divisor = 10;
        multiplier = 1;
    }
}

function plusMin_pressed() {
    entry = entry * -1;
    result.value = entry;
}

function equal_pressed() {
    update_result();
    result.value = accum;
}