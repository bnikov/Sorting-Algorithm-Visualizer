using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;


namespace Sorting_Algorithm_Visualizer {
    public partial class Form1 : Form {

        public int[] arr { get; set; }

        public int size { get; set; }

        public int delay { get; set; }

        Color c;

        public Form1() {
            InitializeComponent();
            DoubleBuffered = true;
            c = Color.White;
        }


        // validating array size textbox  
        private void tbSize_Validating(object sender, CancelEventArgs e) {
            bool hasLetters = false;
            foreach (char c in tbSize.Text) {
                if (!char.IsDigit(c)) {
                    hasLetters = true;
                }
            }
            if (hasLetters || tbSize.Text.Length == 0 || int.Parse(tbSize.Text) > 1000) {
                errorProvider1.SetError(tbSize, "Enter a number (Max 1000)");
                e.Cancel = true;
            }
            else {
                errorProvider1.SetError(tbSize, null);
                e.Cancel = false;
            }
        }

        // validating sorting algorithm dropdown
        private void cbAlgorithm_Validating(object sender, CancelEventArgs e) {
            string input = cbAlgorithm.Text;
            bool valid = false;
            foreach (string s in cbAlgorithm.Items) {
                if (input == s) {
                    valid = true;
                }
            }
            if (valid) {
                errorProvider1.SetError(cbAlgorithm, null);
                e.Cancel = false;
            }
            else {
                errorProvider1.SetError(cbAlgorithm, "Select a sorting algorithm from the dropdown");
                e.Cancel = true;
            }
        }


        // start sorting button
        private void btnSort_Click(object sender, EventArgs e) {
            btnSort.Enabled = false;
            Random rand = new Random();
            c = Color.White;
            arr = new int[int.Parse(tbSize.Text)];
            size = int.Parse(tbSize.Text);
            delay = (int)nudDelay.Value;

            if (size > 900) {
                this.Width = 1000;
            }

            // creating array using the size as a range
            for (int i = 0; i < size; i++) {
                arr[i] = i + 1;
            }

            // shuffling the array
            for (int i = 0; i < size; i++) {
                int j = rand.Next(size);
                int tmp = arr[i];
                arr[i] = arr[j];
                arr[j] = tmp;
            }

            // checking which algorithm to use
            string selectedAlgorithm = cbAlgorithm.SelectedItem.ToString();

            if (selectedAlgorithm == "Bubble Sort") {
                bubbleSort();
            }

            if (selectedAlgorithm == "QuickSort") {
                quickSort();
            }

            if (selectedAlgorithm == "Selection Sort") {
                selectionSort();
            }

            if (selectedAlgorithm == "Insertion Sort") {
                insertionSort();
            }

            if (selectedAlgorithm == "Merge Sort") {
                mergeSort();
            }

            if (selectedAlgorithm == "Radix Sort") {
                radixSort();
            }

            if (selectedAlgorithm == "Shell Sort") {
                shellSort();
            }

            if (selectedAlgorithm == "Cocktail Sort") {
                cocktailSort();
            }
            Invalidate();
        }


        // drawing the array in bar graph form
        private void Form1_Paint(object sender, PaintEventArgs e) {

            Brush b = new SolidBrush(c);
            int gap = 2;
            if (size > 300) {
                gap = 0;
            }

            for (int i = 0; i < size; i++) {
                int x = i * (this.Width - 10) / size;
                int height = (int)(this.Height * 0.6) * arr[i] / size + 20;
                int y = this.Height - 40 - height;
                int width = this.Width / size - gap;
                e.Graphics.FillRectangle(b, new Rectangle(x, y, width, height));
                if (size <= 30) {
                    e.Graphics.DrawString(arr[i].ToString(), new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point), Brushes.Black, x + width / 2 - 15, this.Height - 70);
                }
            }
            b.Dispose();
        }


        // change delay
        private void nudDelay_ValueChanged(object sender, EventArgs e) {
            delay = (int)nudDelay.Value;
        }


        // ============= sorting algorithms ============= 


        // Bubble Sort
        public async void bubbleSort() {
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size - 1; j++) {
                    if (arr[j] > arr[j + 1]) {
                        int tmp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = tmp;
                        Invalidate();
                        await Task.Delay(delay);
                    }
                }
            }
            c = Color.Green;
            Invalidate();
            btnSort.Enabled = true;
        }

        // QuickSort
        public async void quickSort() {

            await quickSort_(arr, 0, arr.Length - 1);

            c = Color.Green;
            Invalidate();
            btnSort.Enabled = true;

            void swap(int[] arr, int i, int j) {
                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
                Console.WriteLine("a");

            }

            async Task quickSort_(int[] arr, int low, int high) {
                Invalidate();
                await Task.Delay(delay);
                if (low < high) {
                    int pivot = arr[high];
                    int i = (low - 1);

                    for (int j = low; j <= high - 1; j++) {
                        if (arr[j] < pivot) {
                            i++;
                            swap(arr, i, j);
                            Invalidate();
                            await Task.Delay(delay);
                        }
                    }
                    swap(arr, i + 1, high);
                    int pi = (i + 1);
                    await quickSort_(arr, low, pi - 1);
                    await quickSort_(arr, pi + 1, high);
                }
            }
        }


        // Selection Sort
        public async void selectionSort() {
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++) {
                int min_idx = i;
                for (int j = i + 1; j < n; j++) {
                    if (arr[j] < arr[min_idx])
                        min_idx = j;
                }
                int temp = arr[min_idx];
                arr[min_idx] = arr[i];
                arr[i] = temp;
                Invalidate();
                await Task.Delay(delay);
            }
            c = Color.Green;
            Invalidate();
            btnSort.Enabled = true;
        }


        // Insertion Sort
        public async void insertionSort() {
            int n = arr.Length;
            for (int i = 1; i < n; ++i) {
                int key = arr[i];
                int j = i - 1;

                while (j >= 0 && arr[j] > key) {
                    arr[j + 1] = arr[j];
                    j = j - 1;
                    Invalidate();
                    await Task.Delay(delay);
                }
                arr[j + 1] = key;
                Invalidate();
            }
            c = Color.Green;
            Invalidate();
            btnSort.Enabled = true;
        }

        
        // Merge Sort
        async public void mergeSort() {

            await SortMethod(arr, 0, arr.Length - 1);
            c = Color.Green;
            Invalidate();

            async Task MergeMethod(int[] arr, int left, int mid, int right) {
                Invalidate();
                await Task.Delay(delay);
                int[] temp = new int[arr.Length];
                int i, left_end, num_elements, tmp_pos;
                left_end = (mid - 1);
                tmp_pos = left;
                num_elements = (right - left + 1);

                while ((left <= left_end) && (mid <= right)) {
                    if (arr[left] <= arr[mid]) {
                        temp[tmp_pos++] = arr[left++];
                    }
                    else {
                        temp[tmp_pos++] = arr[mid++];
                    }

                }
                while (left <= left_end) {
                    temp[tmp_pos++] = arr[left++];
                }

                while (mid <= right) {
                    temp[tmp_pos++] = arr[mid++];
                }

                for (i = 0; i < num_elements; i++) {
                    Invalidate();
                    await Task.Delay(delay);
                    arr[right] = temp[right];
                    right--;
                }
            }

            async Task SortMethod(int[] arr, int left, int right) {
                int mid;
                if (right > left) {
                    mid = (right + left) / 2;
                    await SortMethod(arr, left, mid);
                    await SortMethod(arr, (mid + 1), right);
                    await MergeMethod(arr, left, (mid + 1), right);
                }
            }
            c = Color.Green;
            Invalidate();
            btnSort.Enabled = true;
        }


        // Radix Sort
        public async void radixSort() {

            await radixsort_(arr, arr.Length);
            c = Color.Green;
            Invalidate();
            btnSort.Enabled = true;

            int getMax(int[] a, int n) {
                int mx = a[0];
                for (int i = 1; i < n; i++) {
                    if (a[i] > mx) {
                        mx = a[i];
                    }
                }
                return mx;
            }

            async Task countSort(int[] arr, int n, int exp) {
                int[] output = new int[n];
                int i;
                int[] count = new int[10];

                for (i = 0; i < 10; i++) {
                    count[i] = 0;
                }

                for (i = 0; i < n; i++) {
                    count[(arr[i] / exp) % 10]++;
                }

                for (i = 1; i < 10; i++) {
                    count[i] += count[i - 1];
                }

                for (i = n - 1; i >= 0; i--) {
                    output[count[(arr[i] / exp) % 10] - 1] = arr[i];
                    count[(arr[i] / exp) % 10]--;
                }

                for (i = 0; i < n; i++) {
                    arr[i] = output[i];
                    Invalidate();
                    await Task.Delay(delay);
                }
            }

            async Task radixsort_(int[] arr, int n) {
                int m = getMax(arr, n);

                for (int exp = 1; m / exp > 0; exp *= 10) {
                    await countSort(arr, n, exp);
                }
            }
        }


        // Shell Sort
        public async void shellSort() {
            int n = arr.Length;
            for (int gap = n / 2; gap > 0; gap /= 2) {
                for (int i = gap; i < n; i += 1) {
                    int temp = arr[i];
                    int j;
                    for (j = i; j >= gap && arr[j - gap] > temp; j -= gap) {
                        arr[j] = arr[j - gap];
                    }
                    arr[j] = temp;
                    Invalidate();
                    await Task.Delay(delay);
                }
            }
            c = Color.Green;
            Invalidate();
            btnSort.Enabled = true;
        }


        // Cocktail Sort
        public async void cocktailSort() {
            bool swapped = true;
            int start = 0;
            int end = arr.Length;

            while (swapped == true) {
                swapped = false;
                for (int i = start; i < end - 1; ++i) {
                    if (arr[i] > arr[i + 1]) {
                        int temp = arr[i];
                        arr[i] = arr[i + 1];
                        arr[i + 1] = temp;
                        swapped = true;
                        Invalidate();
                        await Task.Delay(delay);
                    }
                }
                if (swapped == false) {
                    break;
                }
                swapped = false;
                end--;
                for (int i = end - 1; i >= start; i--) {
                    if (arr[i] > arr[i + 1]) {
                        int temp = arr[i];
                        arr[i] = arr[i + 1];
                        arr[i + 1] = temp;
                        swapped = true;
                        Invalidate();
                        await Task.Delay(delay);
                    }
                }
                start++;
            }
            c = Color.Green;
            Invalidate();
            btnSort.Enabled = true;
        }


    }
}
