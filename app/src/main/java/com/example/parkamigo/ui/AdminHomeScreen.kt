package com.example.parkamigo.ui

import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowBack
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.parkamigo.R

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun AdminHomeScreen(onLogout: () -> Unit) {
    var pantallaActual by remember { mutableStateOf("menu") }

    Scaffold(
        topBar = {
            CenterAlignedTopAppBar(
                title = {
                    Text(
                        text = "_____PARK AMIGO_____",
                        fontSize = 22.sp,
                        fontWeight = FontWeight.Bold
                    )
                }
            )
        }
    ) { innerPadding ->
        Box(
            modifier = Modifier
                .fillMaxSize()
                .padding(innerPadding),
            contentAlignment = Alignment.Center
        ) {
            when (pantallaActual) {
                "menu" -> AdminMainMenu(
                    onSelect = { seleccion -> pantallaActual = seleccion },
                    onLogout = onLogout
                )
                "usuarios" -> PantallaUsuarios { pantallaActual = "menu" }
                "reservas" -> PantallaConBack("Reservas") { pantallaActual = "menu" }
                "estacionamiento" -> PantallaConBack("Estacionamiento") { pantallaActual = "menu" }
                "tarjetas" -> PantallaConBack("Tarjetas") { pantallaActual = "menu" }
            }
        }
    }
}

@Composable
fun AdminMainMenu(onSelect: (String) -> Unit, onLogout: () -> Unit) {
    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp)
    ) {
        Text(
            "Menú Principal",
            fontSize = 24.sp,
            fontWeight = FontWeight.Bold,
            modifier = Modifier.padding(bottom = 16.dp)
        )

        val items = listOf(
            Triple("usuarios", "Usuarios", R.drawable.iconousa),
            Triple("reservas", "Reservas", R.drawable.iconoreserva),
            Triple("estacionamiento", "Estacionamiento", R.drawable.iconoparking),
            Triple("tarjetas", "Tarjetas", R.drawable.iconotarjeta)
        )

        Column {
            for (i in items.indices step 2) {
                Row(
                    modifier = Modifier.fillMaxWidth(),
                    horizontalArrangement = Arrangement.spacedBy(16.dp)
                ) {
                    MenuCard(
                        text = items[i].second,
                        iconResId = items[i].third,
                        modifier = Modifier.weight(1f)
                    ) {
                        onSelect(items[i].first)
                    }
                    if (i + 1 < items.size) {
                        MenuCard(
                            text = items[i + 1].second,
                            iconResId = items[i + 1].third,
                            modifier = Modifier.weight(1f)
                        ) {
                            onSelect(items[i + 1].first)
                        }
                    } else {
                        Spacer(modifier = Modifier.weight(1f))
                    }
                }
                Spacer(modifier = Modifier.height(16.dp))
            }
        }

        Spacer(modifier = Modifier.weight(1f))

        Button(
            onClick = onLogout,
            modifier = Modifier.fillMaxWidth()
        ) {
            Row(
                verticalAlignment = Alignment.CenterVertically
            ) {
                Image(
                    painter = painterResource(id = R.drawable.iconocierresesion),
                    contentDescription = "Cerrar sesión",
                    modifier = Modifier
                        .size(20.dp)
                        .padding(end = 8.dp)
                )
                Text("Cerrar sesión")
            }
        }
    } // Esta llave cierra el Column principal de AdminMainMenu
}

    @Composable
    fun MenuCard(
        text: String,
        iconResId: Int,
        modifier: Modifier = Modifier,
        onClick: () -> Unit
    ) {
        Box(
            modifier = modifier
                .height(120.dp)
                .background(Color(0xFF2196F3), RoundedCornerShape(12.dp))
                .clickable { onClick() },
            contentAlignment = Alignment.Center
        ) {
            Column(
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.Center
            ) {
                val painter = painterResource(id = iconResId)
                Image(
                    painter = painter,
                    contentDescription = text,
                    modifier = Modifier.size(40.dp)
                )
                Spacer(modifier = Modifier.height(8.dp))
                Text(
                    text = text,
                    color = Color.White,
                    fontWeight = FontWeight.Bold,
                    fontSize = 16.sp
                )
            }
        }
    }

    @OptIn(ExperimentalMaterial3Api::class)
    @Composable
    fun PantallaConBack(titulo: String, onBack: () -> Unit) {
        Column(
            modifier = Modifier.fillMaxSize()
        ) {
            TopAppBar(
                title = { Text(titulo) },
                navigationIcon = {
                    IconButton(onClick = onBack) {
                        Icon(
                            imageVector = Icons.Default.ArrowBack,
                            contentDescription = "Atrás"
                        )
                    }
                }
            )
            Box(
                modifier = Modifier.fillMaxSize(),
                contentAlignment = Alignment.Center
            ) {
                Text("Aquí va contenido de $titulo")
            }
        }
    }



