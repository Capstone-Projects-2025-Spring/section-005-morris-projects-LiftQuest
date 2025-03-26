import machine
import time
import network
import socket
import struct
import secrets

# MPU6050 I2C address (0x68 is the default I2C address)
MPU_ADDR = 0x68

# MPU6050 Registers
PWR_MGMT_1 = 0x6B
ACCEL_XOUT_H = 0x3B
GYRO_XOUT_H = 0x43

# Initialize I2C
i2c = machine.I2C(0, scl=machine.Pin(21), sda=machine.Pin(20))

# Connect to Wi-Fi
wlan = network.WLAN(network.STA_IF)
wlan.active(True)
wlan.connect(secrets.SSID, secrets.PASSWORD)

# Wait for connection
while not wlan.isconnected():
    time.sleep(1)

# Print the IP address of the Pico W
ip_address = wlan.ifconfig()[0]
print("Connected to Wi-Fi, IP address:", ip_address)

# Setup TCP serve
# Set up the server
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
server_socket.bind(('0.0.0.0', 12345))  # Listen on all network interfaces
server_socket.listen(1)  # Allow only one connection at a time for simplicity

print("Server listening on port 12345...")

# Wait for a client to connect
client_socket, client_address = server_socket.accept()
print("Client connected:", client_address)

print("Pico W IP address:", wlan.ifconfig()[0])


# Function to write to a register
def write_byte(reg, value):
    i2c.writeto_mem(MPU_ADDR, reg, bytes([value]))

# Function to read a 16-bit value from two registers
def read_word(reg):
    high = i2c.readfrom_mem(MPU_ADDR, reg, 1)
    low = i2c.readfrom_mem(MPU_ADDR, reg + 1, 1)
    value = (high[0] << 8) + low[0]
    if value >= 0x8000:  # Convert to signed 16-bit value
        value -= 0x10000
    return value

# Wake up the MPU6050 (it starts in sleep mode)
write_byte(PWR_MGMT_1, 0)

# Read and send accelerometer and gyroscope data
while True:
    print("Wi-Fi connected:", wlan.isconnected())
    print("Pico W IP address:", wlan.ifconfig()[0])
    # Read accelerometer values (X, Y, Z)
    accel_x = read_word(ACCEL_XOUT_H)
    accel_y = read_word(ACCEL_XOUT_H + 2)
    accel_z = read_word(ACCEL_XOUT_H + 4)

    # Read gyroscope values (X, Y, Z)
    gyro_x = read_word(GYRO_XOUT_H)
    gyro_y = read_word(GYRO_XOUT_H + 2)
    gyro_z = read_word(GYRO_XOUT_H + 4)

    # Pack the data into a binary format (6 floats: 3 accelerometer, 3 gyroscope)
    data = struct.pack('ffffff', accel_x, accel_y, accel_z, gyro_x, gyro_y, gyro_z)

    # Send the data to the Unity client
    client_socket.sendall(data)

    # Print the sensor data (for debugging purposes)
    print("Sending data: Accel X:", accel_x, " Y:", accel_y, " Z:", accel_z)
    print("Gyro X:", gyro_x, " Y:", gyro_y, " Z:", gyro_z)

    time.sleep(0.5)

