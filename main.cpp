#include <iostream>

int main() {
	setlocale(LC_ALL, "RUS");
	int arr[10], min, max, passed, * arr2;

	// Addind 10 ascending numbers
	for (int i = 0; i < 10; i++) {
		std::cin >> arr[i];
	}
	std::cout << "\nPassed numbers:\n";

	// Finding max and min numbers to create an array with right number of elements
	min = arr[0];
	max = arr[0];

	for (int i = 1; i < 10; i++) {
		if (arr[i] > max) {
			max = arr[i];
		}
		if (arr[i] < min) {
			min = arr[i];
		}
	}
	passed = max - min + 1;

	// Create an array that includes all numbers from min to max
	arr2 = new int[passed];
	for (int i = 0; i < passed; i++) {
		arr2[i] = i + min;

		//checking for the abscence and outputing only passed numbers
		bool exists = std::find(std::begin(arr), std::end(arr), arr2[i]) != std::end(arr);
		if (not exists) {
			std::cout << arr2[i] << std::endl;
		}
	}
	return 0;
}
