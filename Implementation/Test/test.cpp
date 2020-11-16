// Example program
#include <iostream>
#include <string>
using namespace std;


int main(){

    string data = "2135";
    string str  = "2134";
    if(str.find(data) != -1 || data.find(str) != -1){
        cout << str + " == " + data <<endl;
    }
    return 0; 
}
