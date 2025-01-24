# Instructions to Build and Run the Project

## 1. Building and Running Containers
To build and run the containers, execute the following command in the root directory of the project (where `docker-compose.yml`  is located):  
**`docker-compose up --build`**  


---

## 2. Application Ports and Access Information

### **Backend Application**  
- **Port:** `8080`  
- **URL:** [http://localhost:8080](http://localhost:8080)
- **Swagger Documentation:** [http://localhost:8080/swagger/index.html](http://localhost:8080/swagger/index.html)

### **Database (PostgreSQL)**  
- **Port:** `5432`  
- **Host:** `localhost`  
- **User:** `root`  
- **Password:** `root`  
- **Database Name:** `filmsdb`

### **Frontend Application**  
- **Port:** `3000`  
- **URL:** [http://localhost:3000](http://localhost:3000)

---

## 3. Unit Tests  
Unit tests are located in the following file:  
`FilmManagementSystem.Tests\FilmServiceTests.cs`  

These tests cover the business logic of the `FilmService` and ensure the correctness of core functionalities.

---

## Results  
Once the containers are running, the backend, frontend, and database will be accessible via the ports specified above.

![image](https://github.com/user-attachments/assets/9cd0ae5e-49c8-47d6-8a79-293ec632ab08)
