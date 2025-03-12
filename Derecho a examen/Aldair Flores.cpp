#include <iostream>
#include <cstdio>
#include <cstring> 

using namespace std;

void mostrarMenu() {
    printf("\n MENU STARBUCKS : \n");
    printf("1. Cafe Americano - $60\n");
    printf("2. Latte - $70\n");
    printf("3. Capuchino - $75\n");
    printf("4. Frappuccino - $70\n");
}

double obtenerPrecio(int precios) {
    switch (precios) {
        case 1: return 60.0;
        case 2: return 70.0;
        case 3: return 75.0;
        case 4: return 70.0;
        default: return 0.0;
    }
}

int main() {
    int precios;
    double costo = 0;
    char continuar[3]; 

    printf("\nBienvenido a Starbucks!\n");

    do {
        mostrarMenu();
        printf("Seleccione una opcion para su pedido : ");
        scanf("%d", &precios);

        double precio = obtenerPrecio(precios);
        if (precio == 0) {
            printf(" Opción inválida. Intente de nuevo.\n");
        } else {
            costo += precio;
            printf("Producto agregado. Total actual: $%.2f\n",costo);
        }

        printf("¿Desea agregar otro producto?: ");
        scanf("%s", continuar);

    } while (strcmp(continuar, "Si") == 0 || strcmp(continuar, "si") == 0);

    printf("\n Total a pagar: $%.2f\n", costo);
    printf(" Procesando pago...\n");
    printf(" Pago exitoso. Gracias por su compra!\n");

    return 0;
}
