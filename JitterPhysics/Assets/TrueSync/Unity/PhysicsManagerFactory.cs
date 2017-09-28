namespace TrueSync {

    /**
     *  @brief Manages physics simulation.
     **/
    public class PhysicsManager {

        /**
         *  @brief Indicates the type of physics simulations: 2D or 3D.
         **/
        public enum PhysicsType {W_3D};

        /**
         *  @brief Returns a proper implementation of {@link IPhysicsManager}.
         **/
        public static IPhysicsManager instance;

        /**
         *  @brief Instantiates a new {@link IPhysicsManager}.
         *  
         *  @param trueSyncConfig Indicates if is 3D world.
         **/
        public static IPhysicsManager New(TrueSyncConfig trueSyncConfig) {

            instance = new PhysicsWorldManager();
            instance.Gravity = trueSyncConfig.gravity3D;
            instance.SpeculativeContacts = trueSyncConfig.speculativeContacts3D;


            return instance;
        }

        /**
         *  @brief Instantiates a 3D physics for tests purpose.
         **/
        internal static void InitTest3D() {
            instance = new PhysicsWorldManager();
            instance.Gravity = new TSVector(0, -10, 0);
            instance.LockedTimeStep = 0.02f;
            instance.Init();
        }
        
    }

}