import tensorflow as tf
from tensorflow.keras.preprocessing.image import ImageDataGenerator
from tensorflow.keras.applications import ResNet50V2
from tensorflow.keras import layers, models
from tensorflow.keras.optimizers import Adam

# Step 1: Load Pretrained ResNet50V2 Model
base_model = ResNet50V2(weights='imagenet', include_top=False, input_shape=(224, 224, 3))

# Step 2: Freeze the Base Model (so its weights aren't updated during fine-tuning)
base_model.trainable = False

# Step 3: Add Custom Classification Layers
model = models.Sequential([
    base_model,
    layers.GlobalAveragePooling2D(),  # Pool the features from the convolutional layers
    layers.Dense(1024, activation='relu'),  # Fully connected layer
    layers.Dense(10, activation='softmax')  # Output layer for 10 classes (adjust as needed)
])

# Step 4: Compile the Model
model.compile(optimizer=Adam(learning_rate=1e-4), loss='categorical_crossentropy', metrics=['accuracy'])

# Step 5: Prepare Your Dataset (Ensure your dataset is properly labeled in folders)
train_datagen = ImageDataGenerator(rescale=1.0/255.0, rotation_range=30, width_shift_range=0.2, height_shift_range=0.2, horizontal_flip=True)
validation_datagen = ImageDataGenerator(rescale=1.0/255.0)

# Replace 'path_to_train' and 'path_to_val' with the actual directories containing your dataset
train_generator = train_datagen.flow_from_directory('path_to_train', target_size=(224, 224), batch_size=32, class_mode='categorical')
validation_generator = validation_datagen.flow_from_directory('path_to_val', target_size=(224, 224), batch_size=32, class_mode='categorical')

# Step 6: Train the Model
model.fit(train_generator, epochs=10, validation_data=validation_generator)

# Step 7: Save the Fine-Tuned Model
model.save('fine_tuned_model.h5')  # Save in .h5 format first

# Step 8: Convert to ONNX
import tf2onnx

# Convert the model to ONNX format
onnx_model = tf2onnx.convert.from_keras(model)
onnx_model.save('fine_tuned_model.onnx')  # Save in .onnx format
