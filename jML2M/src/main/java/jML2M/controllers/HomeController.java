package jML2M.controllers;

import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.stage.Stage;

import java.io.IOException;

public class HomeController {
    public static void create(Stage stage) throws IOException {
        Parent parent = FXMLLoader.load(HomeController.class.getResource("/jML2M/views/home.fxml"));
        stage = stage != null ? stage : new Stage();
        stage.setScene(new Scene(parent));
        stage.show();
    }
}
