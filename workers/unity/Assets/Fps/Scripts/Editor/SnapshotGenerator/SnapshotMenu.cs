using System.IO;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.DeploymentManager;
using UnityEditor;
using UnityEngine;

namespace Fps
{
    public class SnapshotMenu : MonoBehaviour
    {
        public static readonly string DefaultSnapshotPath =
            Path.Combine(Application.dataPath, "../../../snapshots/default.snapshot");

        public static readonly string CloudSnapshotPath =
            Path.Combine(Application.dataPath, "../../../snapshots/cloud.snapshot");

        private static void GenerateSnapshot(Snapshot snapshot)
        {
            var spawner = FpsEntityTemplates.Spawner();
            snapshot.AddEntity(spawner);
        }

        [MenuItem("SpatialOS/Generate FPS Snapshot")]
        private static void GenerateFpsSnapshot()
        {
            var localSnapshot = new Snapshot();
            var cloudSnapshot = new Snapshot();

            GenerateSnapshot(localSnapshot);
            GenerateSnapshot(cloudSnapshot);

            AddHealthPacks(localSnapshot);
            AddHealthPacks(cloudSnapshot);
 
            // The local snapshot is identical to the cloud snapshot, but also includes a simulated player coordinator
            // trigger.
            var simulatedPlayerCoordinatorTrigger = FpsEntityTemplates.SimulatedPlayerCoordinatorTrigger();
            localSnapshot.AddEntity(simulatedPlayerCoordinatorTrigger);

            SaveSnapshot(DefaultSnapshotPath, localSnapshot);
            SaveSnapshot(CloudSnapshotPath, cloudSnapshot);
        }

        private static void SaveSnapshot(string path, Snapshot snapshot)
        {
            snapshot.WriteToFile(path);
            Debug.LogFormat("Successfully generated initial world snapshot at {0}", path);
        }

        private static void AddHealthPacks(Snapshot snapshot)
        {
            // Invoke our static function to create an entity template of our health pack with 100 heath.
            var healthPack = FpsEntityTemplates.HealthPickup(new Vector3f(5, 0, 0), 100);
 
            // Add the entity template to the snapshot.
            snapshot.AddEntity(healthPack);
        }
    }
}
